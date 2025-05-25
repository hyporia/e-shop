using FastEndpoints;
using Microsoft.AspNetCore.Http;
using OrderService.Application.Utils.Abstractions;
using OrderService.Contracts.Commands.Cart;
using System.ComponentModel;

namespace OrderService.Application.Endpoints.CartEndpoints;

/// <summary>
/// Remove item from cart
/// </summary>
[Description("Remove item from cart.")]
public class RemoveItemFromCartEndpoint(ICartRepository cartRepository) : Endpoint<RemoveItemFromCart>
{
    public override void Configure()
    {
        Delete("/api/cart/items/{UserId}/{ProductId}");
        AllowAnonymous(); // TODO: Add authentication when user service is ready
        Description(b => b
            .Produces(200)
            .Produces(400)
            .Produces(404)
            .WithTags("Cart"));
    }

    public override async Task HandleAsync(RemoveItemFromCart command, CancellationToken ct)
    {
        var cart = await cartRepository.GetByUserIdAsync(command.UserId, ct);

        if (cart == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var removeResult = cart.RemoveItem(command.ProductId);
        if (removeResult.IsFailure)
        {
            AddError(removeResult.Error);
            await SendErrorsAsync(400, ct);
            return;
        }

        await cartRepository.SaveAsync(cart, ct);
        await SendOkAsync(ct);
    }
}
