using CSharpFunctionalExtensions;
using Shared.Infra.CQRS;

namespace UserService.Application.InternalCommands;

public class RegisterUser(string email, string username, string password) :
    Command<Result<EmptyCommandResponse, RegisterUser.RegisterUserErrorResponse>>
{
    public string Email { get; init; } = email;
    public string Username { get; init; } = username;
    public string Password { get; init; } = password;

    public class RegisterUserErrorResponse
    {
        public RegisterUserErrorCode Code { get; init; }

        public IReadOnlyDictionary<string, string> IdentityErrors { get; init; } = new Dictionary<string, string>();
    }

    public enum RegisterUserErrorCode
    {
        EmailInUse,
        IdentityError
    }
}