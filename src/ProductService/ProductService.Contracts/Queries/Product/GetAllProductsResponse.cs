namespace ProductService.Contracts.Queries.Product;

public class GetAllProductsResponse
{
    public List<ProductResponseItem> Products { get; set; } = [];
}
