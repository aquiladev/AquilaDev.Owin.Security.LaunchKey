using System.Threading.Tasks;

namespace AquilaDev.Owin.Security.LaunchKey
{
	/// <summary>
	/// Specifies callback methods which the <see cref="T:AquilaDev.Owin.Security.LaunchKey.LaunchKeyAuthenticationMiddleware"/> invokes to enable developer control over the authentication process.
	/// </summary>
	public interface ILaunchKeyAuthenticationProvider
	{
		/// <summary>
		/// Invoked whenever LaunchKey succesfully authenticates a user
		/// </summary>
		/// <param name="context">Contains information about the login session as well as the user <see cref="T:System.Security.Claims.ClaimsIdentity"/>.</param>
		/// <returns>
		/// A <see cref="T:System.Threading.Tasks.Task"/> representing the completed operation.
		/// </returns>
		Task Authenticated(LaunchKeyAuthenticatedContext context);

		/// <summary>
		/// Invoked prior to the <see cref="T:System.Security.Claims.ClaimsIdentity"/> being saved in a local cookie and the browser being redirected to the originally requested URL.
		/// </summary>
		/// <param name="context"/>
		/// <returns>
		/// A <see cref="T:System.Threading.Tasks.Task"/> representing the completed operation.
		/// </returns>
		Task ReturnEndpoint(LaunchKeyReturnEndpointContext context);

		/// <summary>
		/// Called when a Challenge causes a redirect to authorize endpoint in the LaunchKey middleware
		/// </summary>
		/// <param name="context">Contains redirect URI and <see cref="T:Microsoft.Owin.Security.AuthenticationProperties"/> of the challenge </param>
		void ApplyRedirect(LaunchKeyApplyRedirectContext context);
	}
}
