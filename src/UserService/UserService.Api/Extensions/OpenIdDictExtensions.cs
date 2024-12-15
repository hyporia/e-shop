using UserService.Data;

namespace UserService.Api.Extensions;

public static class OpenIdDictExtensions
{
	public static IServiceCollection AddOpenIddict(this IServiceCollection services, bool isDevelopment)
	{
		services.AddOpenIddict()
			// Register the OpenIddict core components.
			.AddCore(options =>
			{
				options.UseEntityFrameworkCore()
					   .UseDbContext<UserDbContext>();
			})
			.AddServer(options =>
			{
				options
					.SetAuthorizationEndpointUris("connect/authorize")
					.SetIntrospectionEndpointUris("introspect")
					.SetTokenEndpointUris("connect/token")
					.SetUserinfoEndpointUris("connect/userinfo")
					.AllowAuthorizationCodeFlow()
					.AllowRefreshTokenFlow()
					.AllowPasswordFlow()
					.AddDevelopmentSigningCertificate()
					.UseAspNetCore()
					.EnableAuthorizationEndpointPassthrough()
					.EnableTokenEndpointPassthrough();

				if (isDevelopment)
				{
					options.AddEphemeralEncryptionKey()
						   .DisableAccessTokenEncryption();
				}
			})
			.AddValidation(options =>
			{
				// Import the configuration from the local OpenIddict server instance.
				options.UseLocalServer();
				// Register the ASP.NET Core host.
				options.UseAspNetCore();
			});

		return services;
	}
}
