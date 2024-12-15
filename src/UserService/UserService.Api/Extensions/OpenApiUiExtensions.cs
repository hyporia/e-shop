using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
namespace UserService.Api.Extensions;

public static class OpenApiUiExtensions
{
	public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
	{
		return services
			.AddSwaggerGen(cfg =>
			{
				var port = configuration["ASPNETCORE_HTTPS_PORT"];
				cfg.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
				{
					Type = SecuritySchemeType.OAuth2,
					Flows = new OpenApiOAuthFlows
					{
						AuthorizationCode = new OpenApiOAuthFlow
						{
							AuthorizationUrl = new Uri($"https://localhost:{port}/connect/authorize"),
							TokenUrl = new Uri($"https://localhost:{port}/connect/token"),
							Scopes = new Dictionary<string, string>
							{
								{ "user_api", "user api scope" }
							},
						},
						Password = new OpenApiOAuthFlow
						{
							TokenUrl = new Uri($"https://localhost:{port}/connect/token"),
							Scopes = new Dictionary<string, string>
							{
								{ "user_api", "user api scope" }
							},
							AuthorizationUrl = new Uri($"https://localhost:{port}/connect/authorize"),

						}
					}
				});

				cfg.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
						},
						Array.Empty<string>()
					}
				});
			});
	}

	public static WebApplication MapScalar(this WebApplication app)
	{
		app.MapOpenApi();
		app.MapScalarApiReference(cfg =>
		{
			cfg.WithOAuth2Authentication(authCfg =>
			{
				authCfg.ClientId = "scalar";
				authCfg.Scopes = ["user_api"];
			});
		});
		return app;
	}
}
