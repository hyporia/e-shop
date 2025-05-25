using FastEndpoints;
using Microsoft.AspNetCore.Http;
using OpenIddict.Validation.AspNetCore;
using OrderService.Application.Utils.Abstractions;
using OrderService.Application.Utils.Extensions;
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
        // Use JWT authentication instead of allowing anonymous access
        AuthSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        Description(b => b
            .Produces(200)
            .Produces(400)
            .Produces(403)
            .WithTags("Cart"));
    }

    public override async Task HandleAsync(AddItemToCart command, CancellationToken ct)
    {
        // Extract user ID from JWT token
        var userId = User.GetUserId();
        if (!userId.HasValue)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        // Get existing cart or create new one
        var cart = await cartRepository.GetByUserIdAsync(userId.Value, ct);

        if (cart == null)
        {
            var createCartResult = Cart.Create(userId.Value);
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
