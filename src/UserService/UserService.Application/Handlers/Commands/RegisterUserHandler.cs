using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Infra.CQRS;
using UserService.Application.InternalCommands;
using UserService.Domain;
using static UserService.Application.InternalCommands.RegisterUser;

namespace UserService.Application.Handlers.Commands;

internal class RegisterUserHandler(UserManager<User> userManager) : IRequestHandler<RegisterUser, Result<EmptyCommandResponse, RegisterUserErrorResponse>>
{
    public async Task<Result<EmptyCommandResponse, RegisterUserErrorResponse>> Handle(RegisterUser request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user != null)
        {
            return Result.Failure<EmptyCommandResponse, RegisterUserErrorResponse>(
                new() { Code = RegisterUserErrorCode.EmailInUse });
        }

        user = new() { UserName = request.Username, Email = request.Email };
        var result = await userManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
            return Result.Success<EmptyCommandResponse, RegisterUserErrorResponse>(new());
        }

        return Result.Failure<EmptyCommandResponse, RegisterUserErrorResponse>(
            new()
            {
                Code = RegisterUserErrorCode.IdentityError,
                // TODO: should I expose the IdentityErrors to the client?
                // probably not, I should see what could be inside the IdentityErrors later
                IdentityErrors = result.Errors.ToDictionary(e => e.Code, e => e.Description)
            });
    }
}