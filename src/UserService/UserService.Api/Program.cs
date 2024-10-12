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
                    { "api1", "resource server scope" }
                }
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
        options.SetAuthorizationEndpointUris("authorize")
               .SetIntrospectionEndpointUris("introspect")
               .SetTokenEndpointUris("token");

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
    app.UseSwaggerUI();
}

app.UseHttpLogging();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<NotFoundMiddleware>();

app.MapControllers();

app.Run();
