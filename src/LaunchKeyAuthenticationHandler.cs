using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin;
using Microsoft.Owin.Infrastructure;
using Microsoft.Owin.Logging;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Infrastructure;
using Newtonsoft.Json.Linq;

namespace AquilaDev.Owin.Security.LaunchKey
{
	internal class LaunchKeyAuthenticationHandler : AuthenticationHandler<LaunchKeyAuthenticationOptions>
	{
		private const string TokenEndpoint = "https://oauth.launchkey.com/access_token";
		private const string AuthorizeEndpoint = "https://oauth.launchkey.com/authorize";

		private readonly ILogger _logger;
		private readonly HttpClient _httpClient;

		public LaunchKeyAuthenticationHandler(HttpClient httpClient, ILogger logger)
		{
			_httpClient = httpClient;
			_logger = logger;
		}

		public override async Task<bool> InvokeAsync()
		{
			bool flag;
			if (Options.CallbackPath.HasValue && Options.CallbackPath == Request.Path)
			{
				flag = await InvokeReturnPathAsync();
			}
			else
			{
				flag = false;
			}
			return flag;
		}

		protected override async Task<AuthenticationTicket> AuthenticateCoreAsync()
		{
			AuthenticationProperties properties = null;
			AuthenticationTicket authenticationTicket;
			try
			{
				string code = null;
				string state = null;
				IReadableStringCollection query = Request.Query;
				IList<string> values = query.GetValues("code");
				if (values != null && values.Count == 1)
				{
					code = values[0];
				}
				values = query.GetValues("state");
				if (values != null && values.Count == 1)
				{
					state = values[0];
				}
				properties = Options.StateDataFormat.Unprotect(state);
				if (properties == null)
				{
					authenticationTicket = null;
				}
				else if (!ValidateCorrelationId(properties, _logger))
				{
					authenticationTicket = new AuthenticationTicket(null, properties);
				}
				else
				{
					var tokenRequestParameters = new List<KeyValuePair<string, string>>
					{
						new KeyValuePair<string, string>("client_id", Options.RocketId),
						new KeyValuePair<string, string>("redirect_uri", GenerateRedirectUri()),
						new KeyValuePair<string, string>("client_secret", Options.ClientSecret),
						new KeyValuePair<string, string>("code", code),
						new KeyValuePair<string, string>("grant_type", "authorization_code")
					};
					FormUrlEncodedContent requestContent = new FormUrlEncodedContent(tokenRequestParameters);
					HttpResponseMessage response =
						await _httpClient.PostAsync(TokenEndpoint, requestContent, Request.CallCancelled);
					response.EnsureSuccessStatusCode();
					string oauthTokenResponse = await response.Content.ReadAsStringAsync();
					JObject oauth2Token = JObject.Parse(oauthTokenResponse);
					string accessToken = oauth2Token.Value<string>("access_token");
					string refreshToken = oauth2Token.Value<string>("refresh_token");
					string expires = oauth2Token.Value<string>("expires_in");
					if (string.IsNullOrWhiteSpace(accessToken))
					{
						_logger.WriteWarning("Access token was not found");
						authenticationTicket = new AuthenticationTicket(null, properties);
					}
					else
					{
						string userId = oauth2Token.Value<string>("user");
						var context = new LaunchKeyAuthenticatedContext(Context, accessToken, refreshToken, expires)
						{
							Identity = new ClaimsIdentity(new[]
							{
								new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", userId,
									"http://www.w3.org/2001/XMLSchema#string", Options.AuthenticationType)
							}, Options.AuthenticationType, "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name",
								"http://schemas.microsoft.com/ws/2008/06/identity/claims/role")
						};
						await Options.Provider.Authenticated(context);
						context.Properties = properties;
						authenticationTicket = new AuthenticationTicket(context.Identity, context.Properties);
					}
				}
			}
			catch (Exception ex)
			{
				_logger.WriteError("Authentication failed", ex);
				authenticationTicket = new AuthenticationTicket(null, properties);
			}
			return authenticationTicket;
		}

		protected override Task ApplyResponseChallengeAsync()
		{
			if (Response.StatusCode != 401)
			{
				return Task.FromResult((object)null);
			}
			AuthenticationResponseChallenge responseChallenge = Helper.LookupChallenge(Options.AuthenticationType, Options.AuthenticationMode);
			if (responseChallenge != null)
			{
				string baseUri = string.Concat(Request.Scheme, Uri.SchemeDelimiter, Request.Host, Request.PathBase);
				string callbackUri = baseUri + Options.CallbackPath;
				AuthenticationProperties properties = responseChallenge.Properties;
				if (string.IsNullOrEmpty(properties.RedirectUri))
				{
					properties.RedirectUri = baseUri + Request.Path + Request.QueryString.Value;
				}
				GenerateCorrelationId(properties);
				string stateEscape = Options.StateDataFormat.Protect(properties);
				string redirectUri = String.Format("{0}?client_id={1}&response_type=code&redirect_uri={2}&state={3}&scope={4}",
					AuthorizeEndpoint,
					Uri.EscapeDataString(Options.RocketId),
					Uri.EscapeDataString(callbackUri),
					Uri.EscapeDataString(stateEscape),
					Uri.EscapeDataString(string.Join(" ", Options.Scope)));
				Options.Provider.ApplyRedirect(new LaunchKeyApplyRedirectContext(Context, Options, properties, redirectUri));
			}
			return Task.FromResult((object)null);
		}

		public async Task<bool> InvokeReturnPathAsync()
		{
			AuthenticationTicket model = await AuthenticateAsync();
			bool flag;
			if (model == null)
			{
				_logger.WriteWarning("Invalid return state, unable to redirect.");
				Response.StatusCode = 500;
				flag = true;
			}
			else
			{
				LaunchKeyReturnEndpointContext context = new LaunchKeyReturnEndpointContext(Context, model)
				{
					SignInAsAuthenticationType = Options.SignInAsAuthenticationType,
					RedirectUri = model.Properties.RedirectUri
				};
				model.Properties.RedirectUri = null;
				await Options.Provider.ReturnEndpoint(context);

				if (context.SignInAsAuthenticationType != null && context.Identity != null)
				{
					ClaimsIdentity claimsIdentity = context.Identity;
					if (!string.Equals(claimsIdentity.AuthenticationType, context.SignInAsAuthenticationType, StringComparison.Ordinal))
					{
						claimsIdentity = new ClaimsIdentity(claimsIdentity.Claims, context.SignInAsAuthenticationType,
							claimsIdentity.NameClaimType, claimsIdentity.RoleClaimType);
					}
					Context.Authentication.SignIn(context.Properties, claimsIdentity);
				}

				if (!context.IsRequestCompleted && context.RedirectUri != null)
				{
					if (context.Identity == null)
					{
						context.RedirectUri = WebUtilities.AddQueryString(context.RedirectUri, "error", "access_denied");
					}
					Response.Redirect(context.RedirectUri);
					context.RequestCompleted();
				}
				flag = context.IsRequestCompleted;
			}
			return flag;
		}

		private string GenerateRedirectUri()
		{
			return String.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host, RequestPathBase, Options.CallbackPath);
		}
	}
}
