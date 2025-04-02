namespace ProductService.Contracts.Queries.Product;

public class GetProductsResponse
{
    public List<ProductResponseItem> Products { get; set; } = [];
}
