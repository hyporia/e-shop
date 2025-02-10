using OrderProcessingSystem.ServiceDefaults;
using ProductService.Data;
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

builder.Services.AddData(builder.Configuration.GetConnectionString("postgresql")!);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapDefaultEndpoints();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();