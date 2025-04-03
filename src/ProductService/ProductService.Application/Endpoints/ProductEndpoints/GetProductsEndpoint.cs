using FastEndpoints;
using Microsoft.AspNetCore.Http;
using ProductService.Application.Utils.Abstractions;
using ProductService.Contracts.Queries.Product;
using ProductService.Domain;

namespace ProductService.Application.Endpoints.ProductEndpoints;

public class GetProductsEndpoint(IQueries<Product> productQueries) : Endpoint<GetProducts, GetProductsResponse>
{
    public override void Configure()
    {
        Get("/api/products");
        AllowAnonymous();
        Description(b => b
            .Produces<GetProductsResponse>(200, "application/json")
            .WithTags("Product"));
    }

    public override async Task HandleAsync(GetProducts query, CancellationToken ct)
    {
        var products = await productQueries.GetAllAsync(ct);
        var response = new GetProductsResponse
        {
            Products = products.Select(x => new ProductResponseItem
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price
            }).ToList()
        };
        await SendAsync(response, cancellation: ct);
    }
}
