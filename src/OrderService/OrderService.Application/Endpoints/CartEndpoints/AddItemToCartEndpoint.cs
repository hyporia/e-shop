using FastEndpoints;
using Microsoft.AspNetCore.Http;
using OrderService.Application.Utils.Abstractions;
using OrderService.Contracts.Commands.Cart;
using OrderService.Domain;
using System.ComponentModel;

namespace OrderService.Application.Endpoints.CartEndpoints;

/// <summary>
/// Add item to cart
/// </summary>
[Description("Add item to cart.")]
public class AddItemToCartEndpoint(ICartRepository cartRepository) : Endpoint<AddItemToCart>
{
    public override void Configure()
    {
        Post("/api/cart/items");
        AllowAnonymous(); // TODO: Add authentication when user service is ready
        Description(b => b
            .Produces(200)
            .Produces(400)
            .WithTags("Cart"));
    }

    public override async Task HandleAsync(AddItemToCart command, CancellationToken ct)
    {
        // Get existing cart or create new one
        var cart = await cartRepository.GetByUserIdAsync(command.UserId, ct);

        if (cart == null)
        {
            var createCartResult = Cart.Create(command.UserId);
            if (createCartResult.IsFailure)
            {
                await SendErrorsAsync(400, ct);
                return;
            }
            cart = createCartResult.Value;
        }

        // Add item to cart
        var addItemResult = cart.AddItem(
            command.ProductId,
            command.ProductName,
            command.Price,
            command.Quantity);

        if (addItemResult.IsFailure)
        {
            AddError(addItemResult.Error);
            await SendErrorsAsync(400, ct);
            return;
        }

        // Save cart
        await cartRepository.SaveAsync(cart, ct);
        await SendOkAsync(ct);
    }
}
