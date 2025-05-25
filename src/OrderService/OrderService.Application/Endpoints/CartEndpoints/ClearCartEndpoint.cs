using FastEndpoints;
using Microsoft.AspNetCore.Http;
using OrderService.Application.Utils.Abstractions;
using OrderService.Contracts.Commands.Cart;
using System.ComponentModel;

namespace OrderService.Application.Endpoints.CartEndpoints;

/// <summary>
/// Clear cart
/// </summary>
[Description("Clear cart.")]
public class ClearCartEndpoint(ICartRepository cartRepository) : Endpoint<ClearCart>
{
    public override void Configure()
    {
        Delete("/api/cart/{UserId}");
        AllowAnonymous(); // TODO: Add authentication when user service is ready
        Description(b => b
            .Produces(200)
            .Produces(404)
            .WithTags("Cart"));
    }

    public override async Task HandleAsync(ClearCart command, CancellationToken ct)
    {
        var cart = await cartRepository.GetByUserIdAsync(command.UserId, ct);

        if (cart == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        cart.Clear();
        await cartRepository.SaveAsync(cart, ct);
        await SendOkAsync(ct);
    }
}
