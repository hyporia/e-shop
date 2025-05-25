using FastEndpoints;
using Microsoft.AspNetCore.Http;
using OrderService.Application.Utils.Abstractions;
using OrderService.Contracts.Queries.Cart;
using OrderService.Domain;
using System.ComponentModel;

namespace OrderService.Application.Endpoints.CartEndpoints;

/// <summary>
/// Get cart by user ID
/// </summary>
[Description("Get cart by user ID.")]
public class GetCartByUserIdEndpoint(ICartRepository cartRepository) :
    EndpointWithMapping<GetCartByUserId, CartResponse?, Cart?>
{
    public override void Configure()
    {
        Get("/api/cart/{UserId}");
        AllowAnonymous(); // TODO: Add authentication when user service is ready
        Description(b => b
            .Produces<CartResponse>(200)
            .Produces(404)
            .WithTags("Cart"));
    }

    public override async Task HandleAsync(GetCartByUserId query, CancellationToken ct)
    {
        var cart = await cartRepository.GetByUserIdAsync(query.UserId, ct);

        if (cart == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendOkAsync(MapFromEntity(cart), ct);
    }

    public override CartResponse? MapFromEntity(Cart? cart)
    {
        if (cart == null) return null;

        return new CartResponse
        {
            Id = cart.Id,
            UserId = cart.UserId,
            Items = cart.Items.Select(item => new CartItemResponse
            {
                Id = item.Id,
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Price = item.Price,
                Quantity = item.Quantity,
                AddedAt = item.AddedAt,
                TotalPrice = item.GetTotalPrice()
            }).ToList(),
            CreatedAt = cart.CreatedAt,
            UpdatedAt = cart.UpdatedAt,
            TotalAmount = cart.GetTotalAmount(),
            TotalItemCount = cart.GetTotalItemCount()
        };
    }
}
