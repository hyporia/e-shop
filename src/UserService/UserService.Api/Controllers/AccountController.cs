using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.InternalCommands;
using static UserService.Application.InternalCommands.RegisterUser;

namespace UserService.Api.Controllers;

[ApiController]
public class AccountController(IMediator mediator) : ControllerBase
{
    // POST: /Account/Register
    [HttpPost("/account/register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterUser request, CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(request, cancellationToken);

        if (result.IsSuccess)
        {
            return Created();
        }

        if (result.Error.Count == 1 && result.Error.ContainsKey(RegisterUserErrorCode.DuplicateEmail))
        {
            return Problem(
                title: "The email is already taken.",
                statusCode: StatusCodes.Status409Conflict,
                detail: "The email is already taken."
            );
        }

        return Problem(
            title: "One or more errors occurred during registration.",
            statusCode: StatusCodes.Status400BadRequest,
            detail: "One or more errors occurred during registration.",
            extensions: result.Error.ToDictionary(kvp => kvp.Key.ToString(), kvp => (object?)kvp.Value)
        );
    }
}
