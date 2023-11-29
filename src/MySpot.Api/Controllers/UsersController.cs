using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MySpot.Application.Abstractions;
using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Application.Queries;
using MySpot.Application.Security;
using MySpot.Core.ValueObjects;
using Swashbuckle.AspNetCore.Annotations;

namespace MySpot.Api.Controllers;

[ApiController]
[Route("[Controller]")]
public class UsersController : ControllerBase
{
    private readonly ICommandHandler<SignUp> _signUpHandler;
    private readonly ICommandHandler<SignIn> _signInHandler;
    private readonly IQueryHandler<GetUser, UserDto> _getUserhandler;
    private readonly IQueryHandler<GetUsers, IEnumerable<UserDto>> _getUsershandler;
    private readonly ITokenStorage _tokenStorage;

    public UsersController(
        ICommandHandler<SignUp> signUpHandler,
        ICommandHandler<SignIn> signInHandler, 
        IQueryHandler<GetUser, UserDto> getUserHandler,
        IQueryHandler<GetUsers, IEnumerable<UserDto>> getUsersHandler,
        ITokenStorage tokenStorage)
    {
        _signUpHandler = signUpHandler;
        _signInHandler = signInHandler;
        _getUserhandler = getUserHandler;
        _getUsershandler = getUsersHandler;
        _tokenStorage = tokenStorage;
    }

    [HttpPost]
    public async Task<ActionResult> Post(SignUp command)
    {
        await _signUpHandler.HandleAsync(command with { UserId = Guid.NewGuid() });
        return CreatedAtAction(nameof(Get), new {command.UserId}, null);
    }

    [HttpPost("sign-in")]
    public async Task<ActionResult> Post(SignIn command)
    {
        await _signInHandler.HandleAsync(command);
        var jwt = _tokenStorage.Get();

        return Ok(jwt);
    }


    [HttpGet("{userId:guid}")]
    [Authorize(Policy = "is-admin")]
    [SwaggerOperation("Get single user by ID if exists.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserDto>> Get(Guid userId)
    {
        var user = await _getUserhandler.HandleAsync(new GetUser { UserId = userId });
        if (user is null)
            return NotFound();

        return user;
    }

    [HttpGet("me")]
    [Authorize(Policy = "is-admin")]
    public async Task<ActionResult<UserDto>> Get()
    {
        if (string.IsNullOrEmpty(HttpContext.User.Identity?.Name))
            return NotFound();

        var userId = Guid.Parse(HttpContext.User.Identity.Name);
        var user = await _getUserhandler.HandleAsync(new GetUser { UserId = userId }); 
        if (user is null)
            return NotFound();

        return user;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> Get([FromQuery] GetUsers query)
        => Ok(await _getUsershandler.HandleAsync(query));
}
