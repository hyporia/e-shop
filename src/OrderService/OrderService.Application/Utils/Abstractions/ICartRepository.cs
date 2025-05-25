using OrderService.Domain;

namespace OrderService.Application.Utils.Abstractions;

public interface ICartRepository
{
    Task<Cart?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<Cart> SaveAsync(Cart cart, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid cartId, CancellationToken cancellationToken = default);
}
