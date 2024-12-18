using MediatR;
using Microsoft.AspNetCore.Identity;
using UserService.Application.InternalCommands;
using UserService.Domain;

namespace UserService.Application.Handlers.Commands;

internal class LogoutUserHandler(SignInManager<User> signInManager) : IRequestHandler<LogoutUser, string>
{
    public async Task<string> Handle(LogoutUser request, CancellationToken cancellationToken)
    {
        await signInManager.SignOutAsync();

        if (string.IsNullOrEmpty(request.RedirectUri) || !Uri.IsWellFormedUriString(request.RedirectUri, UriKind.Absolute))
        {
            return new("/");
        }

        return new(request.RedirectUri);
    }
}