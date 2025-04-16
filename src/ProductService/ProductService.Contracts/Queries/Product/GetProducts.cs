namespace ProductService.Contracts.Queries.Product;

public class GetProducts
{
    public string? Name { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public string? Description { get; set; }
}
