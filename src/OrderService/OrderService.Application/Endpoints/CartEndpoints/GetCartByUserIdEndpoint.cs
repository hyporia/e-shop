using FastEndpoints;
using Microsoft.AspNetCore.Http;
using OpenIddict.Validation.AspNetCore;
using OrderService.Application.Utils.Abstractions;
using OrderService.Application.Utils.Extensions;
using OrderService.Contracts.Queries.Cart;
using OrderService.Domain;
using System.ComponentModel;

namespace OrderService.Application.Endpoints.CartEndpoints;

/// <summary>
/// Get cart by user ID
/// </summary>
[Description("Get cart by user ID.")]
public class GetCartByUserIdEndpoint(ICartRepository cartRepository) :
    EndpointWithoutRequest<CartResponse?>
{
    public override void Configure()
    {
        Get("/api/cart");

        // Use JWT authentication instead of allowing anonymous access
        AuthSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);

        Description(b => b
            .Produces<CartResponse>(200)
            .Produces(404)
            .WithTags("Cart"));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        // Extract user ID from JWT token
        var userId = User.GetUserId();
        if (!userId.HasValue)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var cart = await cartRepository.GetByUserIdAsync(userId.Value, ct);

        if (cart == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendOkAsync(MapFromEntity(cart), ct);
    }

    public static CartResponse? MapFromEntity(Cart? cart)
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
