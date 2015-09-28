using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Provider;

namespace AquilaDev.Owin.Security.LaunchKey
{
	/// <summary>
	/// Provides context information to middleware providers.
	/// </summary>
	public class LaunchKeyReturnEndpointContext : ReturnEndpointContext
	{
		/// <summary>
		/// Initializes a new <see cref="T:AquilaDev.Owin.Security.LaunchKey.LaunchKeyReturnEndpointContext"/>.
		/// </summary>
		/// <param name="context">OWIN environment</param><param name="ticket">The authentication ticket</param>
		public LaunchKeyReturnEndpointContext(IOwinContext context, AuthenticationTicket ticket) : base(context, ticket)
		{
		}
	}
}
