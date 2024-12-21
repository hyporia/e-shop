using CSharpFunctionalExtensions;
using OpenIddict.Abstractions;
using Shared.Infra.CQRS;
using System.Security.Claims;

namespace UserService.Application.InternalCommands;

public class ExchangeUserCredentials(OpenIddictRequest openIddictRequest) : Command<Result<ClaimsPrincipal, string>>
{
    public OpenIddictRequest OpenIddictRequest { get; set; } = openIddictRequest;
}