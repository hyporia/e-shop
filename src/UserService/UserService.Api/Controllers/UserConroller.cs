using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Infra.CQRS;
using UserService.UseCases;

namespace UserService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserConroller : ControllerBase
{
    private readonly IMediator _mediator;

    public UserConroller(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<EmptyCommandResponse> Create([FromBody] RegisterUser request)
    {
        return await _mediator.Send(request);
    }
}
