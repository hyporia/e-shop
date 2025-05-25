namespace OrderService.Contracts.Commands.Cart;

public class AddItemToCart
{
    /// <summary>
    /// The user ID who owns the cart.
    /// </summary>
    public required Guid UserId { get; set; }

    /// <summary>
    /// The product ID to add to cart.
    /// </summary>
    public required Guid ProductId { get; set; }

    /// <summary>
    /// The product name.
    /// </summary>
    public required string ProductName { get; set; }

    /// <summary>
    /// The product price.
    /// </summary>
    public required decimal Price { get; set; }

    /// <summary>
    /// The quantity to add to cart.
    /// </summary>
    public required int Quantity { get; set; }
}

public class UpdateCartItemQuantity
{
    /// <summary>
    /// The user ID who owns the cart.
    /// </summary>
    public required Guid UserId { get; set; }

    /// <summary>
    /// The product ID to update.
    /// </summary>
    public required Guid ProductId { get; set; }

    /// <summary>
    /// The new quantity.
    /// </summary>
    public required int Quantity { get; set; }
}

public class RemoveItemFromCart
{
    /// <summary>
    /// The user ID who owns the cart.
    /// </summary>
    public required Guid UserId { get; set; }

    /// <summary>
    /// The product ID to remove from cart.
    /// </summary>
    public required Guid ProductId { get; set; }
}

public class ClearCart
{
    /// <summary>
    /// The user ID who owns the cart.
    /// </summary>
    public required Guid UserId { get; set; }
}
