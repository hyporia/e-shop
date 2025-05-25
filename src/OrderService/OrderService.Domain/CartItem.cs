using CSharpFunctionalExtensions;

namespace OrderService.Domain;

public class CartItem : Entity<Guid>
{
    public Guid CartId { get; private set; }
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; }
    public decimal Price { get; private set; }
    public int Quantity { get; private set; }
    public DateTime AddedAt { get; private set; }

    private CartItem(Guid cartId, Guid productId, string productName, decimal price, int quantity)
    {
        CartId = cartId;
        ProductId = productId;
        ProductName = productName;
        Price = price;
        Quantity = quantity;
        AddedAt = DateTime.UtcNow;
    }

    public static Result<CartItem> Create(Guid cartId, Guid productId, string productName, decimal price, int quantity)
    {
        if (cartId == Guid.Empty)
        {
            return Result.Failure<CartItem>("CartId cannot be empty");
        }

        if (productId == Guid.Empty)
        {
            return Result.Failure<CartItem>("ProductId cannot be empty");
        }

        if (string.IsNullOrWhiteSpace(productName))
        {
            return Result.Failure<CartItem>("Product name cannot be empty");
        }

        if (price <= 0)
        {
            return Result.Failure<CartItem>("Price must be greater than 0");
        }

        if (quantity <= 0)
        {
            return Result.Failure<CartItem>("Quantity must be greater than 0");
        }

        return Result.Success(new CartItem(cartId, productId, productName, price, quantity));
    }

    public Result UpdateQuantity(int newQuantity)
    {
        if (newQuantity <= 0)
        {
            return Result.Failure("Quantity must be greater than 0");
        }

        Quantity = newQuantity;
        return Result.Success();
    }

    public decimal GetTotalPrice()
    {
        return Price * Quantity;
    }
}
