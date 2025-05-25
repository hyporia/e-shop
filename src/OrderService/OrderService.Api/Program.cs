using EShop.ServiceDefaults;
using FastEndpoints;
using FastEndpoints.Swagger;
using OrderService.Api.Extensions;
using OrderService.Application.Endpoints.CartEndpoints;
using OrderService.Data.Extensions;
using OpenIddict.Validation.AspNetCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddEndpointsApiExplorer();
builder.Services
   .AddFastEndpoints(x => x.Assemblies = [typeof(GetCartByUserIdEndpoint).Assembly])
   .SwaggerDocument(x =>
   {
       x.NewtonsoftSettings = s =>
       {
           s.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
       };
   });

builder.Services.AddData(builder.Configuration.GetConnectionString("OrderDb")!);

// Add OpenIddict validation to validate JWT tokens issued by UserService
builder.Services.AddOpenIddictValidation(builder.Configuration);

// Configure authentication with OpenIddict as the default scheme
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
    options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
});

// Add authorization services
builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000", policyBuilder =>
    {
        policyBuilder.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi(c => c.Path = "/openapi/{documentName}.json");
    app.MapScalarApiReference();
}

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseCors("AllowLocalhost3000");

// Add authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerGen();
}

app.Run();