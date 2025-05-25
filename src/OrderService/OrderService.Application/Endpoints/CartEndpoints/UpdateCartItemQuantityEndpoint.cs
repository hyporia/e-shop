using FastEndpoints;
using Microsoft.AspNetCore.Http;
using OrderService.Application.Utils.Abstractions;
using OrderService.Contracts.Commands.Cart;
using System.ComponentModel;

namespace OrderService.Application.Endpoints.CartEndpoints;

/// <summary>
/// Update cart item quantity
/// </summary>
[Description("Update cart item quantity.")]
public class UpdateCartItemQuantityEndpoint(ICartRepository cartRepository) : Endpoint<UpdateCartItemQuantity>
{
    public override void Configure()
    {
        Put("/api/cart/items");
        AllowAnonymous(); // TODO: Add authentication when user service is ready
        Description(b => b
            .Produces(200)
            .Produces(400)
            .Produces(404)
            .WithTags("Cart"));
    }

    public override async Task HandleAsync(UpdateCartItemQuantity command, CancellationToken ct)
    {
        var cart = await cartRepository.GetByUserIdAsync(command.UserId, ct);

        if (cart == null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var updateResult = cart.UpdateItemQuantity(command.ProductId, command.Quantity);
        if (updateResult.IsFailure)
        {
            AddError(updateResult.Error);
            await SendErrorsAsync(400, ct);
            return;
        }

        await cartRepository.SaveAsync(cart, ct);
        await SendOkAsync(ct);
    }
}
