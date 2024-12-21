using OpenIddict.Abstractions;
using Shared.Infra.CQRS;
using System.Security.Claims;

namespace UserService.Application.InternalCommands;

public class IssueAuthorizationCode(OpenIddictRequest request) : Command<ClaimsPrincipal>
{
    public OpenIddictRequest OpenIddictRequest { get; } = request;
}