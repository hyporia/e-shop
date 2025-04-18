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
[Description("Get products.")]
public class GetProductsEndpoint(IQueries<Product> productQueries) :
    EndpointWithMapping<GetProducts, ProductResponseItem[], IEnumerable<Product>>
{
    public override void Configure()
    {
        Get("/api/products");
        AllowAnonymous();
        Description(b => b
            .Produces<ProductResponseItem[]>(200, "application/json")
            .WithTags("Product"));
    }

    public override async Task HandleAsync(GetProducts query, CancellationToken ct)
    {
        var products = await productQueries.GetAllAsync(ct);
        await SendAsync(MapFromEntity(products), cancellation: ct);
    }

    public override ProductResponseItem[] MapFromEntity(IEnumerable<Product> e) =>
        e.Select(x => new ProductResponseItem
        {
            Id = x.Id,
            Name = x.Name,
            Price = x.Price
        }).ToArray();
}
