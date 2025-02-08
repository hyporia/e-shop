using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Authentication;
using OpenIddict.Abstractions;
using Shared.Infra.CQRS;
using System.Security.Claims;

namespace UserService.Application.InternalCommands;
public class ExchangeRefreshToken(OpenIddictRequest openIddictRequest, AuthenticateResult authenticateResult) : Command<Result<ClaimsPrincipal, string>>
{
    public OpenIddictRequest OpenIddictRequest { get; set; } = openIddictRequest;

    public AuthenticateResult AuthenticateResult { get; set; } = authenticateResult;
}