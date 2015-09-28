using System;
using AquilaDev.Owin.Security.LaunchKey;

namespace Owin
{
	/// <summary>
	/// Extension methods for using <see cref="T:AquilaDev.Owin.Security.LaunchKey.LaunchKeyAuthenticationMiddleware"/>
	/// </summary>
	public static class LaunchKeyAuthenticationExtensions
	{
		public static IAppBuilder UseLaunchKeyAuthentication(this IAppBuilder app, LaunchKeyAuthenticationOptions options)
		{
			if (app == null)
			{
				throw new ArgumentNullException("app");
			}
			if (options == null)
			{
				throw new ArgumentNullException("options");
			}
			app.Use(typeof(LaunchKeyAuthenticationMiddleware), (object)app, (object)options);
			return app;
		}

		public static IAppBuilder UseLaunchKeyAuthentication(this IAppBuilder app, string rocketId, string clientSecret)
		{
			return UseLaunchKeyAuthentication(app, new LaunchKeyAuthenticationOptions
			{
				RocketId = rocketId,
				ClientSecret = clientSecret
			});
		}
	}
}
