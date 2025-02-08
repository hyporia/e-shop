using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace UserService.Api.OpenAPI;

internal class ServersTransformer(IConfiguration configuration) : IOpenApiDocumentTransformer
{
    public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context,
        CancellationToken cancellationToken)
    {
        var port = configuration["ASPNETCORE_HTTPS_PORT"];
        document.Servers = new List<OpenApiServer>
        {
            new() { Url = $"https://localhost:{port}", Description = "Development" }
        };

        return Task.CompletedTask;
    }
}