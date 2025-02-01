using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace UserService.Api.OpenAPI;

internal class AuthorizationTransformer(IConfiguration configuration) : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
    {
        var port = configuration["ASPNETCORE_HTTPS_PORT"];
        var requirements = new Dictionary<string, OpenApiSecurityScheme>
        {
            ["OAuth2"] = new()
            {
                Type = SecuritySchemeType.OAuth2,
                Scheme = "oauth2",
                Name = "Authorization",
                In = ParameterLocation.Header,
                OpenIdConnectUrl = new Uri($"https://localhost:{port}/.well-known/openid-configuration"),
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
                        AuthorizationUrl = new Uri($"https://localhost:{port}/connect/authorize"),
                        TokenUrl = new Uri($"https://localhost:{port}/connect/token"),
                        Scopes = new Dictionary<string, string>
                        {
                            { "user_api", "user api scope" },
                            { "offline_access", "offline access for refresh token" }
                        },
                    },
                }
            },
        };
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes = requirements;

        return Task.CompletedTask;
    }
}
