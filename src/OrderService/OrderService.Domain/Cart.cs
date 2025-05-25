using CSharpFunctionalExtensions;

namespace OrderService.Domain;

public class Cart : Entity<Guid>
{
    public Guid UserId { get; private set; }
    public List<CartItem> Items { get; private set; } = new();
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private Cart(Guid userId)
    {
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public static Result<Cart> Create(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            return Result.Failure<Cart>("UserId cannot be empty");
        }

        return Result.Success(new Cart(userId));
    }

    public Result AddItem(Guid productId, string productName, decimal price, int quantity)
    {
        if (productId == Guid.Empty)
        {
            return Result.Failure("ProductId cannot be empty");
        }

        if (string.IsNullOrWhiteSpace(productName))
        {
            return Result.Failure("Product name cannot be empty");
        }

        if (price <= 0)
        {
            return Result.Failure("Price must be greater than 0");
        }

        if (quantity <= 0)
        {
            return Result.Failure("Quantity must be greater than 0");
        }

        var existingItem = Items.FirstOrDefault(i => i.ProductId == productId);
        if (existingItem != null)
        {
            var updateResult = existingItem.UpdateQuantity(existingItem.Quantity + quantity);
            if (updateResult.IsFailure)
            {
                return updateResult;
            }
        }
        else
        {
            var cartItemResult = CartItem.Create(Id, productId, productName, price, quantity);
            if (cartItemResult.IsFailure)
            {
                return Result.Failure(cartItemResult.Error);
            }

            Items.Add(cartItemResult.Value);
        }

        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }

    public Result UpdateItemQuantity(Guid productId, int quantity)
    {
        var item = Items.FirstOrDefault(i => i.ProductId == productId);
        if (item == null)
        {
            return Result.Failure("Item not found in cart");
        }

        var updateResult = item.UpdateQuantity(quantity);
        if (updateResult.IsFailure)
        {
            return updateResult;
        }

        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }

    public Result RemoveItem(Guid productId)
    {
        var item = Items.FirstOrDefault(i => i.ProductId == productId);
        if (item == null)
        {
            return Result.Failure("Item not found in cart");
        }

        Items.Remove(item);
        UpdatedAt = DateTime.UtcNow;
        return Result.Success();
    }

    public void Clear()
    {
        Items.Clear();
        UpdatedAt = DateTime.UtcNow;
    }

    public decimal GetTotalAmount()
    {
        return Items.Sum(i => i.GetTotalPrice());
    }

    public int GetTotalItemCount()
    {
        return Items.Sum(i => i.Quantity);
    }
}
