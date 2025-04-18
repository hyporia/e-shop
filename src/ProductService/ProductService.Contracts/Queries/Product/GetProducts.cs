namespace ProductService.Contracts.Queries.Product;

public class GetProducts
{
    public string? Name { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}

public class ProductResponseItem
{
    /// <summary>
    /// The product ID.
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    /// The product name.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// The product price.
    /// </summary>
    public required decimal Price { get; set; }
}
