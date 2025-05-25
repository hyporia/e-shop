namespace OrderService.Contracts.Queries.Cart;

public class GetCartByUserId
{
    // No properties needed - user ID will be extracted from JWT token
}

public class CartResponse
{
    /// <summary>
    /// The cart ID.
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    /// The user ID who owns the cart.
    /// </summary>
    public required Guid UserId { get; set; }

    /// <summary>
    /// List of items in the cart.
    /// </summary>
    public required List<CartItemResponse> Items { get; set; }

    /// <summary>
    /// When the cart was created.
    /// </summary>
    public required DateTime CreatedAt { get; set; }

    /// <summary>
    /// When the cart was last updated.
    /// </summary>
    public required DateTime UpdatedAt { get; set; }

    /// <summary>
    /// Total amount of all items in the cart.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Total count of all items in the cart.
    /// </summary>
    public int TotalItemCount { get; set; }
}

public class CartItemResponse
{
    /// <summary>
    /// The cart item ID.
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    /// The product ID.
    /// </summary>
    public required Guid ProductId { get; set; }

    /// <summary>
    /// The product name.
    /// </summary>
    public required string ProductName { get; set; }

    /// <summary>
    /// The product price at the time it was added to cart.
    /// </summary>
    public required decimal Price { get; set; }

    /// <summary>
    /// The quantity of the product in the cart.
    /// </summary>
    public required int Quantity { get; set; }

    /// <summary>
    /// When the item was added to the cart.
    /// </summary>
    public required DateTime AddedAt { get; set; }

    /// <summary>
    /// Total price for this item (Price * Quantity).
    /// </summary>
    public decimal TotalPrice { get; set; }
}
