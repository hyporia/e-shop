using Microsoft.AspNetCore.Mvc;
using UserService.Controllers;
using UserService.Events;

namespace UserService.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserConroller : ControllerBase
{
    private readonly IEventPublisher _eventPublisher;

    public UserConroller(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateUserRequest request)
    {
        var userCreatedEvent = new UserCreatedEvent(request.Name, request.Email);
        _eventPublisher.Publish(userCreatedEvent);
        return Ok();
    }
}
