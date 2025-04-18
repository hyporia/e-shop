namespace ProductService.Contracts.Queries.Product;

public class GetProductById
{
    public Guid Id { get; set; }
}

public class GetProductByIdResponse
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

    /// <summary>
    /// The product description.
    /// </summary>
    public required string Description { get; set; }
}