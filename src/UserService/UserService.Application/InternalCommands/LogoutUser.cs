using Shared.Infra.CQRS;

namespace UserService.Application.InternalCommands;

/// <summary>
/// Command to logout a user.
/// Returns a redirect URI
/// </summary>
public class LogoutUser(string? redirectUri) : Command<string>
{
    public string? RedirectUri { get; set; } = redirectUri;
}