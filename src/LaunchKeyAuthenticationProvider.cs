using System;
using System.Threading.Tasks;

namespace AquilaDev.Owin.Security.LaunchKey
{
	/// <summary>
	/// Default <see cref="T:AquilaDev.Owin.Security.LaunchKey.ILaunchKeyAuthenticationProvider"/> implementation.
	/// </summary>
	public class LaunchKeyAuthenticationProvider : ILaunchKeyAuthenticationProvider
	{
		/// <summary>
		/// Gets or sets the function that is invoked when the Authenticated method is invoked.
		/// </summary>
		public Func<LaunchKeyAuthenticatedContext, Task> OnAuthenticated { get; set; }

		/// <summary>
		/// Gets or sets the function that is invoked when the ReturnEndpoint method is invoked.
		/// </summary>
		public Func<LaunchKeyReturnEndpointContext, Task> OnReturnEndpoint { get; set; }

		/// <summary>
		/// Gets or sets the delegate that is invoked when the ApplyRedirect method is invoked.
		/// </summary>
		public Action<LaunchKeyApplyRedirectContext> OnApplyRedirect { get; set; }

		/// <summary>
		/// Initializes a new <see cref="T:AquilaDev.Owin.Security.LaunchKey.LaunchKeyAuthenticationProvider"/>
		/// </summary>
		public LaunchKeyAuthenticationProvider()
		{
			OnAuthenticated = context => (Task)Task.FromResult((object)null);
			OnReturnEndpoint = context => (Task)Task.FromResult((object)null);
			OnApplyRedirect = context => context.Response.Redirect(context.RedirectUri);
		}

		/// <summary>
		/// Invoked whenever LaunchKey succesfully authenticates a user
		/// </summary>
		/// <param name="context">Contains information about the login session as well as the user <see cref="T:System.Security.Claims.ClaimsIdentity"/>.</param>
		/// <returns>
		/// A <see cref="T:System.Threading.Tasks.Task"/> representing the completed operation.
		/// </returns>
		public Task Authenticated(LaunchKeyAuthenticatedContext context)
		{
			return OnAuthenticated(context);
		}

		/// <summary>
		/// Invoked prior to the <see cref="T:System.Security.Claims.ClaimsIdentity"/> being saved in a local cookie and the browser being redirected to the originally requested URL.
		/// </summary>
		/// <param name="context">Contains information about the login session as well as the user <see cref="T:System.Security.Claims.ClaimsIdentity"/></param>
		/// <returns>
		/// A <see cref="T:System.Threading.Tasks.Task"/> representing the completed operation.
		/// </returns>
		public Task ReturnEndpoint(LaunchKeyReturnEndpointContext context)
		{
			return OnReturnEndpoint(context);
		}

		/// <summary>
		/// Called when a Challenge causes a redirect to authorize endpoint in the LaunchKey account middleware
		/// </summary>
		/// <param name="context">Contains redirect URI and <see cref="T:Microsoft.Owin.Security.AuthenticationProperties"/> of the challenge </param>
		public void ApplyRedirect(LaunchKeyApplyRedirectContext context)
		{
			OnApplyRedirect(context);
		}
	}
}
