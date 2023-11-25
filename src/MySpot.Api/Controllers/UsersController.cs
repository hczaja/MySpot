using Microsoft.AspNetCore.Mvc;
using MySpot.Application.Abstractions;
using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Application.Queries;

namespace MySpot.Api.Controllers;

[ApiController]
[Route("[Controller]")]
public class UsersController : ControllerBase
{
    private readonly ICommandHandler<SignUp> _signUpHandler;
    private readonly IQueryHandler<GetUser, UserDto> _getUserhandler;
    private readonly IQueryHandler<GetUsers, IEnumerable<UserDto>> _getUsershandler;

    public UsersController(
        ICommandHandler<SignUp> signUpHandler, 
        IQueryHandler<GetUser, UserDto> getUserHandler,
        IQueryHandler<GetUsers, IEnumerable<UserDto>> getUsersHandler)
    {
        _signUpHandler = signUpHandler;
        _getUserhandler = getUserHandler;
        _getUsershandler = getUsersHandler;
    }

    [HttpPost]
    public async Task<ActionResult> Post(SignUp command)
    {
        await _signUpHandler.HandleAsync(command with { UserId = Guid.NewGuid() });
        return NoContent();
    }

    [HttpGet("{userId:guid}")]
    public async Task<ActionResult<UserDto>> Get(Guid userId)
    {
        var user = await _getUserhandler.HandleAsync(new GetUser { UserId = userId });
        if (user is null)
            return NotFound();

        return user;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> Get([FromQuery] GetUsers query)
        => Ok(await _getUsershandler.HandleAsync(query));
}
