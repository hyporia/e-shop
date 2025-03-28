using EShop.ServiceDefaults;
using MediatR;
using ProductService.Application.Extensions;
using ProductService.Contracts.Queries.Product;
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
builder.Services.AddApplication();

builder.Services.AddData(builder.Configuration.GetConnectionString("productDb")!);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapDefaultEndpoints();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGet("/products", async ([AsParameters] GetAllProducts query, IMediator mediator, CancellationToken cancellationToken) =>
{
    var result = await mediator.Send(query, cancellationToken);
    return Results.Ok(result);
});

app.Run();
