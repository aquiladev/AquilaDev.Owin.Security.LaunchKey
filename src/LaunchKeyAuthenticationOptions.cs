using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace AquilaDev.Owin.Security.LaunchKey
{
	/// <summary>
	/// Configuration options for <see cref="T:AquilaDev.Owin.Security.LaunchKey.LaunchKeyAuthenticationMiddleware"/>
	/// </summary>
	public class LaunchKeyAuthenticationOptions : AuthenticationOptions
	{
		/// <summary>
		/// Get or sets the text that the user can display on a sign in user interface.
		/// </summary>
		/// <remarks>
		/// The default value is 'LaunchKey'
		/// </remarks>
		public string Caption
		{
			get
			{
				return Description.Caption;
			}
			set
			{
				Description.Caption = value;
			}
		}

		/// <summary>
		/// The application rocket ID assigned by the LaunchKey authentication service.
		/// </summary>
		public string RocketId { get; set; }

		/// <summary>
		/// The application client secret assigned by the LaunchKey authentication service.
		/// </summary>
		public string ClientSecret { get; set; }

		/// <summary>
		/// Gets or sets timeout value in milliseconds for back channel communications with LaunchKey.
		/// </summary>
		/// <value>
		/// The back channel timeout.
		/// </value>
		public TimeSpan BackchannelTimeout { get; set; }

		/// <summary>
		/// A list of permissions to request.
		/// </summary>
		public IList<string> Scope { get; private set; }

		/// <summary>
		/// The request path within the application's base path where the user-agent will be returned.
		///             The middleware will process this request when it arrives.
		///             Default value is "/signin-launchkey".
		/// </summary>
		public PathString CallbackPath { get; set; }

		/// <summary>
		/// Gets or sets the name of another authentication middleware which will be responsible for actually issuing a user <see cref="T:System.Security.Claims.ClaimsIdentity"/>.
		/// </summary>
		public string SignInAsAuthenticationType { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="T:AquilaDev.Owin.Security.LaunchKey.ILaunchKeyAuthenticationProvider"/> used to handle authentication events.
		/// </summary>
		public ILaunchKeyAuthenticationProvider Provider { get; set; }

		/// <summary>
		/// Gets or sets the type used to secure data handled by the middleware.
		/// </summary>
		public ISecureDataFormat<AuthenticationProperties> StateDataFormat { get; set; }

		/// <summary>
		/// Gets or sets the a pinned certificate validator to use to validate the endpoints used
		///             in back channel communications belong to LaunchKey.
		/// </summary>
		/// <value>
		/// The pinned certificate validator.
		/// </value>
		/// <remarks>
		/// If this property is null then the default certificate checks are performed,
		///             validating the subject name and if the signing chain is a trusted party.
		/// </remarks>
		public ICertificateValidator BackchannelCertificateValidator { get; set; }

		/// <summary>
		/// The HttpMessageHandler used to communicate with LaunchKey.
		///             This cannot be set at the same time as BackchannelCertificateValidator unless the value
		///             can be downcast to a WebRequestHandler.
		/// </summary>
		public HttpMessageHandler BackchannelHttpHandler { get; set; }

		/// <summary>
		/// Initializes a new <see cref="T:AquilaDev.Owin.Security.LaunchKey.LaunchKeyAuthenticationOptions"/>.
		/// </summary>
		public LaunchKeyAuthenticationOptions()
			: base("LaunchKey")
		{
			Caption = "LaunchKey";
			CallbackPath = new PathString("/signin-launchkey");
			AuthenticationMode = AuthenticationMode.Passive;
			Scope = new List<string>();
			BackchannelTimeout = TimeSpan.FromSeconds(60.0);
		}
	}
}
