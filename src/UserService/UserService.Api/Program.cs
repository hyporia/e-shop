using MassTransit;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OrderProcessingSystem.ServiceDefaults;
using System.Reflection;
using UserService.Data;
using UserService.Data.Extensions;
using UserService.Handlers.Extensions;
using UserService.Api.Middleware;
using UserService.Api.Workers;
using OpenIddict.Abstractions;
using System.Security.Claims;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(cfg =>
{
    var port = builder.Configuration["ASPNETCORE_HTTPS_PORT"];
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
builder.Services.AddUseCaseHandlers();
builder.Services.AddDatabase(builder.Configuration.GetConnectionString("postgresql")!);
builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.SetInMemorySagaRepositoryProvider();

    var entryAssembly = Assembly.GetEntryAssembly();

    x.AddConsumers(entryAssembly);
    x.AddSagaStateMachines(entryAssembly);
    x.AddSagas(entryAssembly);
    x.AddActivities(entryAssembly);

    //builder.Services.Configure<MassTransitHostOptions>(options =>
    //{
    //    options.WaitUntilStarted = true;
    //});
    x.UsingRabbitMq((context, cfg) =>
    {
        var configService = context.GetRequiredService<IConfiguration>();
        var connectionString = configService.GetConnectionString("rabbitmq");
        cfg.Host(connectionString);

        cfg.ConfigureEndpoints(context);
    });
});
builder.Services.AddOpenIddict()
    // Register the OpenIddict core components.
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore()
            .UseDbContext<UserDbContext>();
    })

    // Register the OpenIddict server components.
    .AddServer(options =>
    {
        // Enable the authorization, introspection and token endpoints.
        options.SetAuthorizationEndpointUris("connect/authorize")
               .SetIntrospectionEndpointUris("introspect")
               .SetTokenEndpointUris("connect/token");

        // Note: this sample only uses the authorization code and refresh token
        // flows but you can enable the other flows if you need to support implicit,
        // password or client credentials.
        options.AllowAuthorizationCodeFlow()
            .AllowRefreshTokenFlow();

        // Register the encryption credentials. This sample uses a symmetric
        // encryption key that is shared between the server and the Api2 sample
        // (that performs local token validation instead of using introspection).
        //
        // Note: in a real world application, this encryption key should be
        // stored in a safe place (e.g in Azure KeyVault, stored as a secret).
        options.AddEncryptionKey(new SymmetricSecurityKey(
            Convert.FromBase64String("DRjd/GnduI3Efzen9V9BvbNUfc/VKgXltV7Kbk9sMkY=")));

        // Register the signing credentials.
        options.AddDevelopmentSigningCertificate();

        // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
        //
        // Note: unlike other samples, this sample doesn't use token endpoint pass-through
        // to handle token requests in a custom MVC action. As such, the token requests
        // will be automatically handled by OpenIddict, that will reuse the identity
        // resolved from the authorization code to produce access and identity tokens.
        //
        options.UseAspNetCore()
            .EnableAuthorizationEndpointPassthrough();

    })

    // Register the OpenIddict validation components.
    .AddValidation(options =>
    {
        // Import the configuration from the local OpenIddict server instance.
        options.UseLocalServer();

        // Register the ASP.NET Core host.
        options.UseAspNetCore();
    });

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddHostedService<DevelopmentAuthorizationDataSeeder>();
}

builder.Services.AddHttpLogging(x =>
{
    x.LoggingFields = HttpLoggingFields.RequestPath | HttpLoggingFields.RequestMethod | HttpLoggingFields.ResponseStatusCode;
});

builder.Services.AddCors();
builder.Services.AddAuthorization();
var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(cfg =>
    {
        cfg.OAuthClientId("swagger");
        cfg.OAuthUsePkce();
        // cfg.OAuthClientSecret("secret");
    });
}

app.UseMiddleware<NotFoundMiddleware>();

app.UseHttpLogging();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapMethods("connect/authorize", [HttpMethods.Get, HttpMethods.Post], async (HttpContext context, IOpenIddictScopeManager manager) =>
{
    // Retrieve the OpenIddict server request from the HTTP context.
    var request = context.GetOpenIddictServerRequest();

    // Create the claims-based identity that will be used by OpenIddict to generate tokens.
    var identity = new ClaimsIdentity(
        authenticationType: TokenValidationParameters.DefaultAuthenticationType,
        nameType: Claims.Name,
        roleType: Claims.Role);

    identity.AddClaim(new Claim(Claims.Subject, request.ClientId!));
    // Note: in this sample, the client is granted all the requested scopes for the first identity (Alice)
    // but for the second one (Bob), only the "api1" scope can be granted, which will cause requests sent
    // to Zirku.Api2 on behalf of Bob to be automatically rejected by the OpenIddict validation handler,
    // as the access token representing Bob won't contain the "resource_server_2" audience required by Api2.
    identity.SetScopes(request.GetScopes());


    identity.SetResources(await manager.ListResourcesAsync(identity.GetScopes()).ToListAsync());

    // Allow all claims to be added in the access tokens.
    identity.SetDestinations(claim => [Destinations.AccessToken]);

    return Results.SignIn(new ClaimsPrincipal(identity), properties: null, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
});

app.Run();