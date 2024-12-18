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

        return result.Error.Code switch
        {
            RegisterUserErrorCode.EmailInUse => Conflict(),
            RegisterUserErrorCode.IdentityError => BadRequest(result.Error.IdentityErrors),
            _ => BadRequest()
        };
    }
}