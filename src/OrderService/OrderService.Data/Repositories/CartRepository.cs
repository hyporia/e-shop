using Microsoft.EntityFrameworkCore;
using OrderService.Application.Utils.Abstractions;
using OrderService.Domain;

namespace OrderService.Data.Repositories;

internal class CartRepository : ICartRepository
{
    private readonly OrderDbContext _context;

    public CartRepository(OrderDbContext context)
    {
        _context = context;
    }

    public async Task<Cart?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);
    }

    public async Task<Cart> SaveAsync(Cart cart, CancellationToken cancellationToken = default)
    {
        var existingCart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.Id == cart.Id, cancellationToken);

        if (existingCart == null)
        {
            _context.Carts.Add(cart);
        }
        else
        {
            _context.Entry(existingCart).CurrentValues.SetValues(cart);

            // Update cart items
            foreach (var item in cart.Items)
            {
                var existingItem = existingCart.Items.FirstOrDefault(i => i.Id == item.Id);
                if (existingItem == null)
                {
                    existingCart.Items.Add(item);
                }
                else
                {
                    _context.Entry(existingItem).CurrentValues.SetValues(item);
                }
            }

            // Remove items that are no longer in the cart
            var itemsToRemove = existingCart.Items
                .Where(ei => !cart.Items.Any(i => i.Id == ei.Id))
                .ToList();

            foreach (var item in itemsToRemove)
            {
                existingCart.Items.Remove(item);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);
        return cart;
    }

    public async Task DeleteAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var cart = await _context.Carts
            .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);

        if (cart != null)
        {
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
