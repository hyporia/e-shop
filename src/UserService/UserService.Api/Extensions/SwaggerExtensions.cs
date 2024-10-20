using Microsoft.OpenApi.Models;

namespace UserService.Api.Extensions;

public static class SwaggerExtensions
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
}
