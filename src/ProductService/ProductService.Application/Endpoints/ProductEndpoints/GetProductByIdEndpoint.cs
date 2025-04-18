using FastEndpoints;
using Microsoft.AspNetCore.Http;
using ProductService.Application.Utils.Abstractions;
using ProductService.Contracts.Queries.Product;
using ProductService.Domain;
using System.ComponentModel;

namespace ProductService.Application.Endpoints.ProductEndpoints;

/// <summary>
/// Get products
/// </summary>
/// <param name="productQueries"></param>
[Description("Get product by id.")]
public class GetProductByIdEndpoint(IQueries<Product> productQueries) :
    EndpointWithMapping<GetProductById, GetProductByIdResponse, Product>
{
    public override void Configure()
    {
        Get("/api/products/{id:guid}");
        AllowAnonymous();
        Description(b => b
            .Produces<GetProductByIdResponse>(StatusCodes.Status200OK, "application/json")
            .Produces(StatusCodes.Status404NotFound)
            .WithTags("Product"));
    }

    public override async Task HandleAsync(GetProductById query, CancellationToken ct)
    {
        var product = await productQueries.GetByIdAsync(query.Id, ct);
        if (product is null)
        {
            await SendNotFoundAsync(ct);
        }
        else
        {
            await SendAsync(MapFromEntity(product), StatusCodes.Status200OK, ct);
        }
    }

    public override GetProductByIdResponse MapFromEntity(Product e) =>
        new()
        {
            Id = e.Id,
            Name = e.Name,
            Description = e.Description,
            Price = e.Price
        };
}