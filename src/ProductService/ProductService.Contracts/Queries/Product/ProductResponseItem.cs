namespace ProductService.Contracts.Queries.Product;

public class ProductResponseItem
{
    /// <summary>
    /// The product ID.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The product name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The product price.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// The product description.
    /// </summary>
    public string Description { get; set; }
}
