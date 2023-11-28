using System.Security.Authentication;
using MySpot.Application.Abstractions;
using MySpot.Application.Exceptions;
using MySpot.Application.Security;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.Services;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Commands.Handlers;

internal sealed class SignInHandler : ICommandHandler<SignIn>
{
    private readonly IAuthenticator _authenticator;
    private readonly IPasswordManager _passwordManager;
    private readonly IUserRepository _userRepository;
    private readonly ITokenStorage _tokenStorage;

    public SignInHandler(
        IPasswordManager passwordManager, 
        IAuthenticator authenticator, 
        IUserRepository userRepository,
        ITokenStorage tokenStorage)
    {
        _passwordManager = passwordManager;
        _userRepository = userRepository;
        _authenticator = authenticator;
        _tokenStorage = tokenStorage;
    }

    public async Task HandleAsync(SignIn command)
    {
        var user = await _userRepository.GetUserByEmailAsync(command.Email);
        if (user is null)
            throw new InvalidCredentialException();

        if (!_passwordManager.Validate(command.Password, user.Password))
            throw new InvalidCredentialException();
        
        var jwt = _authenticator.CreateToken(user.Id, user.Role);
        _tokenStorage.Set(jwt);
    }
}
