using Scalar.AspNetCore;
namespace UserService.Api.Extensions;

public static class ScalarExtensions
{
    public static WebApplication MapScalar(this WebApplication app)
    {
        app.MapScalarApiReference(cfg =>
        {
            cfg.Servers = [];
            cfg.WithOAuth2Authentication(authCfg =>
            {
                authCfg.ClientId = "scalar";
                authCfg.Scopes = ["user_api"];
            });
        });
        return app;
    }
}