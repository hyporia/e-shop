using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Validation.AspNetCore;
using OrderProcessingSystem.ServiceDefaults;
using UserService.Api.Extensions;
using UserService.Api.Middleware;
using UserService.Api.OpenAPI;
using UserService.Api.Workers;
using UserService.Application.Extensions;
using UserService.Contracts.Queries.User;
using UserService.Data;
using UserService.Data.Extensions;
using UserService.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<AuthorizationTransformer>();
    options.AddDocumentTransformer<ServersTransformer>();
});
builder.Services.AddApplication();
builder.Services.AddDatabase(builder.Configuration.GetConnectionString("postgresql")!);
builder.Services.AddMassTransit();

builder.Services.AddIdentity<User, IdentityRole>(x =>
    {
        x.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<UserDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(builder.Configuration.GetSection(nameof(IdentityOptions)));

builder.Services.AddOpenIddict(builder.Environment.IsDevelopment());

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddHostedService<DevelopmentAuthorizationDataSeeder>();
}

builder.Services.AddHttpLogging(x =>
{
    x.LoggingFields = HttpLoggingFields.RequestPath | HttpLoggingFields.RequestMethod |
                      HttpLoggingFields.ResponseStatusCode;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000", policyBuilder =>
    {
        policyBuilder.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalar();
}

app.UseMiddleware<NotFoundMiddleware>();

app.UseHttpLogging();

app.UseHttpsRedirection();

app.UseCors("AllowLocalhost3000");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/user",
    [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
    (IMediator mediator,
        CancellationToken cancellationToken) => mediator.Send(new GetUsers(), cancellationToken)
);

app.Run();