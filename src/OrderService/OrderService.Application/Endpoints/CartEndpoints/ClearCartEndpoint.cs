using FastEndpoints;
using Microsoft.AspNetCore.Http;
using OpenIddict.Validation.AspNetCore;
using OrderService.Application.Utils.Abstractions;
using OrderService.Application.Utils.Extensions;
using OrderService.Contracts.Commands.Cart;
using System.ComponentModel;

namespace OrderService.Application.Endpoints.CartEndpoints;

/// <summary>
/// Clear cart
/// </summary>
[Description("Clear cart.")]
public class ClearCartEndpoint(ICartRepository cartRepository) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Delete("/api/cart");
        // Use JWT authentication instead of allowing anonymous access
        AuthSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        Description(b => b
            .Produces(200)
            .Produces(404)
            .Produces(403)
            .WithTags("Cart"));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
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

        cart.Clear();
        await cartRepository.SaveAsync(cart, ct);
        await SendOkAsync(ct);
    }
}
