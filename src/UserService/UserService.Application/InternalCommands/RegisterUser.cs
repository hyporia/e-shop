using CSharpFunctionalExtensions;
using Shared.Infra.CQRS;
using static UserService.Application.InternalCommands.RegisterUser;

namespace UserService.Application.InternalCommands;

public class RegisterUser(string email, string username, string password) :
    Command<Result<EmptyCommandResponse, Dictionary<RegisterUserErrorCode, string>>>
{
    public string Email { get; init; } = email;
    public string Username { get; init; } = username;
    public string Password { get; init; } = password;

    public enum RegisterUserErrorCode
    {
        /// <summary>
        /// The password is shorter than the minimum required length.
        /// </summary>
        PasswordTooShort,

        /// <summary>
        /// The password does not contain a non-alphanumeric character.
        /// </summary>
        PasswordRequiresNonAlphanumeric,

        /// <summary>
        /// The password does not contain a numeric digit.
        /// </summary>
        PasswordRequiresDigit,

        /// <summary>
        /// The password does not contain a lowercase letter.
        /// </summary>
        PasswordRequiresLower,

        /// <summary>
        /// The password does not contain an uppercase letter.
        /// </summary>
        PasswordRequiresUpper,

        /// <summary>
        /// The email is already taken.
        /// </summary>
        DuplicateEmail,

        /// <summary>
        /// The username is invalid.
        /// </summary>
        InvalidUserName,

        /// <summary>
        /// The email is invalid.
        /// </summary>
        InvalidEmail
    }
}
