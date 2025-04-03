using EShop.ServiceDefaults;
using FastEndpoints;
using ProductService.Data.Extensions;
using Scalar.AspNetCore;
using Shared.Api.OpenAPI;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<ServersTransformer>();
});
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddFastEndpoints();

builder.Services.AddData(builder.Configuration.GetConnectionString("productDb")!);
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
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapDefaultEndpoints();

app.UseHttpsRedirection();

app.UseCors("AllowLocalhost3000");

app.UseAuthorization();

app.UseFastEndpoints();

app.Run();
