using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Provider;

namespace AquilaDev.Owin.Security.LaunchKey
{
	/// <summary>
	/// Context passed when a Challenge causes a redirect to authorize endpoint in the LaunchKey middleware
	/// </summary>
	public class LaunchKeyApplyRedirectContext : BaseContext<LaunchKeyAuthenticationOptions>
	{
		/// <summary>
		/// Gets the URI used for the redirect operation.
		/// </summary>
		public string RedirectUri { get; private set; }

		/// <summary>
		/// Gets the authenticaiton properties of the challenge
		/// </summary>
		public AuthenticationProperties Properties { get; private set; }

		/// <summary>
		/// Creates a new context object.
		/// </summary>
		/// <param name="context">The OWIN request context</param>
		/// <param name="options">The LaunchKey account middleware options</param>
		/// <param name="properties">The authenticaiton properties of the challenge</param>
		/// <param name="redirectUri">The initial redirect URI</param>
		public LaunchKeyApplyRedirectContext(IOwinContext context, LaunchKeyAuthenticationOptions options, AuthenticationProperties properties, string redirectUri)
			: base(context, options)
		{
			RedirectUri = redirectUri;
			Properties = properties;
		}
	}
}
