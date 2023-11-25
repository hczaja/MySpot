using MySpot.Application.Abstractions;
using MySpot.Application.Exceptions;
using MySpot.Application.Security;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.Services;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Commands.Handlers;

internal sealed class SignUpHandler : ICommandHandler<SignUp>
{
    private readonly IClock _clock;
    private readonly IPasswordManager _passwordManager;
    private readonly IUserRepository _userRepository;

    public SignUpHandler(IPasswordManager passwordManager, IUserRepository userRepository, IClock clock)
    {
        _passwordManager = passwordManager;
        _userRepository = userRepository;
        _clock = clock;
    }

    public async Task HandleAsync(SignUp command)
    {
        // validate input
        var userId = new UserId(command.UserId);
        var email = new Email(command.Email);
        var username = new Username(command.Username);
        var fullname = new FullName(command.FullName);
        var password = new Password(command.Password);
        var role = string.IsNullOrEmpty(command.Role) ? Role.User() : new Role(command.Role);

        // validate if user already exists
        if (await _userRepository.GetUserByEmailAsync(email) is not null)
            throw new EmailAlreadyInUseException();

        if (await _userRepository.GetUserByUsernameAsync(username) is not null)
            throw new UsernameAlreadyInUseException();

        // create user
        var securedPassword = _passwordManager.Secure(password.Value);
        var user = new User(userId.Value, email.Value,
            username.Value, securedPassword, fullname.Value, 
            role.Value, _clock.Current().Value.Date);

        // save to db
        await _userRepository.AddAsync(user);
    }
}
