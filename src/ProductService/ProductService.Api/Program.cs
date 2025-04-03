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

app.MapGet("/products", async ([AsParameters] GetProducts query, IMediator mediator, CancellationToken cancellationToken) =>
{
    var result = await mediator.Send(query, cancellationToken);
    return Results.Ok(result);
})
    .Produces<GetProductsResponse>();

app.Run();
