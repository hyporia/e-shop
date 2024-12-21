using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Infra.CQRS;
using UserService.Application.InternalCommands;
using UserService.Domain;
using static UserService.Application.InternalCommands.RegisterUser;

namespace UserService.Application.Handlers.Commands;

internal class RegisterUserHandler(UserManager<User> userManager)
    : IRequestHandler<RegisterUser, Result<EmptyCommandResponse, Dictionary<RegisterUserErrorCode, string>>>
{
    public async Task<Result<EmptyCommandResponse, Dictionary<RegisterUserErrorCode, string>>> Handle(RegisterUser request, CancellationToken cancellationToken)
    {
        var user = new User() { UserName = request.Username, Email = request.Email };
        var result = await userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
            return new EmptyCommandResponse();
        }

        return GetErrors(result);
    }

    private static Dictionary<RegisterUserErrorCode, string> GetErrors(IdentityResult identityResult)
        => identityResult.Errors.ToDictionary(
            error => Enum.Parse<RegisterUserErrorCode>(error.Code),
            error => error.Description
        );
}