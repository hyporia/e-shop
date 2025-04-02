using MediatR;
using ProductService.Application.Utils.Abstractions;
using ProductService.Contracts.Queries.Product;
using ProductService.Domain;

namespace ProductService.Application.Handlers.Queries;
internal class GetProductsHandler(IQueries<Product> productQueries) : IRequestHandler<GetProducts, GetProductsResponse>
{
    public async Task<GetProductsResponse> Handle(GetProducts request, CancellationToken cancellationToken)
    {
        var products = await productQueries.GetAllAsync(cancellationToken);
        return new()
        {
            Products = products.Select(x => new ProductResponseItem
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price
            }).ToList()
        };
    }
}
