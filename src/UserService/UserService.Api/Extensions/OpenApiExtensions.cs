using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace UserService.Api.Extensions;

internal static class OpenApiExtensions
{
    public static void Configure(OpenApiOptions options) => options.AddDocumentTransformer<AuthorizationTransformer>();

    internal class AuthorizationTransformer(IConfiguration configuration) : IOpenApiDocumentTransformer
    {
        public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
        {
            var port = configuration["ASPNETCORE_HTTPS_PORT"];
            var requirements = new Dictionary<string, OpenApiSecurityScheme>
            {
                ["OAuth2"] = new OpenApiSecurityScheme
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
                            TokenUrl = new Uri($"https://localhost:{port}/connect/token"),
                            Scopes = new Dictionary<string, string>
                                        {
                                            { "user_api", "user api scope" }
                                        },
                            AuthorizationUrl = new Uri($"https://localhost:{port}/connect/authorize"),
                        }
                    }
                },
            };
            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes = requirements;

            return Task.CompletedTask;
        }
    }
}