using System;
using System.Globalization;
using System.Net.Http;
using Microsoft.Owin;
using Microsoft.Owin.Logging;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.Infrastructure;
using Owin;

namespace AquilaDev.Owin.Security.LaunchKey
{
	/// <summary>
	/// OWIN middleware for authenticating users using the LaunchKey service
	/// </summary>
	public class LaunchKeyAuthenticationMiddleware : AuthenticationMiddleware<LaunchKeyAuthenticationOptions>
	{
		private readonly ILogger _logger;
		private readonly HttpClient _httpClient;

		public LaunchKeyAuthenticationMiddleware(OwinMiddleware next, IAppBuilder app, LaunchKeyAuthenticationOptions options)
			: base(next, options)
		{
			if (string.IsNullOrWhiteSpace(Options.RocketId))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
					Resources.Exception_OptionMustBeProvided, "RocketId"));
			}
			if (string.IsNullOrWhiteSpace(Options.ClientSecret))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture,
					Resources.Exception_OptionMustBeProvided, "ClientSecret"));
			}

			_logger = app.CreateLogger<LaunchKeyAuthenticationMiddleware>();
			if (Options.Provider == null)
				Options.Provider = new LaunchKeyAuthenticationProvider();
			if (Options.StateDataFormat == null)
				Options.StateDataFormat = new PropertiesDataFormat(app.CreateDataProtector(typeof(LaunchKeyAuthenticationMiddleware).FullName, Options.AuthenticationType, "v1"));
			if (string.IsNullOrEmpty(Options.SignInAsAuthenticationType))
				Options.SignInAsAuthenticationType = app.GetDefaultSignInAsAuthenticationType();
			_httpClient = new HttpClient(ResolveHttpMessageHandler(Options))
			{
				Timeout = Options.BackchannelTimeout,
				MaxResponseContentBufferSize = 10485760L
			};
		}

		/// <summary>
		/// Provides the <see cref="T:Microsoft.Owin.Security.Infrastructure.AuthenticationHandler"/> object for processing authentication-related requests.
		/// </summary>
		/// <returns>
		/// An <see cref="T:Microsoft.Owin.Security.Infrastructure.AuthenticationHandler"/> configured with the <see cref="T:AquilaDev.Owin.Security.LaunchKey.LaunchKeyAuthenticationOptions"/> supplied to the constructor.
		/// </returns>
		protected override AuthenticationHandler<LaunchKeyAuthenticationOptions> CreateHandler()
		{
			return new LaunchKeyAuthenticationHandler(_httpClient, _logger);
		}

		private static HttpMessageHandler ResolveHttpMessageHandler(LaunchKeyAuthenticationOptions options)
		{
			HttpMessageHandler httpMessageHandler = options.BackchannelHttpHandler ?? new WebRequestHandler();
			if (options.BackchannelCertificateValidator != null)
			{
				WebRequestHandler webRequestHandler = httpMessageHandler as WebRequestHandler;
				if (webRequestHandler == null)
					throw new InvalidOperationException(Resources.Exception_ValidatorHandlerMismatch);
				webRequestHandler.ServerCertificateValidationCallback = options.BackchannelCertificateValidator.Validate;
			}
			return httpMessageHandler;
		}
	}
}
