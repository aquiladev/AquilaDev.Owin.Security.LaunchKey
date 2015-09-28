using System;
using System.Globalization;
using System.Security.Claims;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Provider;

namespace AquilaDev.Owin.Security.LaunchKey
{
	/// <summary>
	/// Contains information about the login session as well as the user <see cref="T:System.Security.Claims.ClaimsIdentity"/>.
	/// </summary>
	public class LaunchKeyAuthenticatedContext : BaseContext
	{
		/// <summary>
		/// Gets the access token provided by the LaunchKey authenication service
		/// </summary>
		public string AccessToken { get; private set; }

		/// <summary>
		/// Gets the refresh token provided by LaunchKey authentication service
		/// </summary>
		public string RefreshToken { get; private set; }

		/// <summary>
		/// Gets the LaunchKey access token expiration time
		/// </summary>
		public TimeSpan? ExpiresIn { get; set; } 

		/// <summary>
		/// Gets the <see cref="T:System.Security.Claims.ClaimsIdentity"/> representing the user
		/// </summary>
		public ClaimsIdentity Identity { get; set; }

		/// <summary>
		/// Gets or sets a property bag for common authentication properties
		/// </summary>
		public AuthenticationProperties Properties { get; set; }

		public LaunchKeyAuthenticatedContext(IOwinContext context, string accessToken, string refreshToken, string expires)
			: base(context)
		{
			AccessToken = accessToken;
			RefreshToken = refreshToken;
			int result;
			if (int.TryParse(expires, NumberStyles.Integer, CultureInfo.InvariantCulture, out result))
			{
				ExpiresIn = TimeSpan.FromSeconds(result);
			}
		}
	}
}
