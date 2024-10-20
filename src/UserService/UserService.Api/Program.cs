using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OrderProcessingSystem.ServiceDefaults;
using UserService.Api.Extensions;
using UserService.Api.Middleware;
using UserService.Api.Workers;
using UserService.Contracts.Queries.User;
using UserService.Data;
using UserService.Data.Extensions;
using UserService.Domain;
using UserService.Application.Extensions;
using OpenIddict.Validation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddDatabase(builder.Configuration.GetConnectionString("postgresql")!);
builder.Services.AddMassTransit();

// Register the Identity services.
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<UserDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(builder.Configuration.GetSection(nameof(IdentityOptions)));

builder.Services.AddOpenIddict(builder.Environment.IsDevelopment());

if (builder.Environment.IsDevelopment())
    builder.Services.AddHostedService<DevelopmentAuthorizationDataSeeder>();

builder.Services.AddHttpLogging(x =>
{
    x.LoggingFields = HttpLoggingFields.RequestPath | HttpLoggingFields.RequestMethod | HttpLoggingFields.ResponseStatusCode;
});

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000", builder =>
    {
        builder.WithOrigins("http://localhost:3000")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
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
        cfg.OAuthUsername("test");
    });
}

app.UseMiddleware<NotFoundMiddleware>();

app.UseHttpLogging();

app.UseHttpsRedirection();

app.UseCors("AllowLocalhost3000");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/user", [Authorize(AuthenticationSchemes = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)]
(IMediator mediator, CancellationToken cancellationToken) => mediator.Send(new GetUsers(), cancellationToken)
);

app.Run();
