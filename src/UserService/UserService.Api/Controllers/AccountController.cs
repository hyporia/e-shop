using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.InternalCommands;
using UserService.Domain;

namespace UserService.Api.Controllers;

[ApiController]
public class AccountController(UserManager<User> userManager) : ControllerBase
{
    // POST: /Account/Register
    [HttpPost("/account/register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterUser request)
    {
        var user = await userManager.FindByNameAsync(request.Email);
        if (user != null)
        {
            return StatusCode(StatusCodes.Status409Conflict);
        }

        user = new User { UserName = request.Email, Email = request.Email };
        var result = await userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
            return Ok();
        }

        return BadRequest(result.Errors);
    }
}