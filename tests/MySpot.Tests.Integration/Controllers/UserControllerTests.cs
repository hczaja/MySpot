using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.DependencyInjection;
using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;
using MySpot.Infrastructure.Security;
using MySpot.Infrastructure.Services;
using Shouldly;

namespace MySpot.Tests.Integration.Controllers;

public class UserControllerTests : ControllerTests, IDisposable
{

    private readonly TestDatabase _testDatabase;
    private IUserRepository _userRepository;
    const string password = "secret";

    public UserControllerTests()
        : base(new OptionsProvider())
    {
        _testDatabase = new TestDatabase();
    }

    [Test]
    public async Task post_users_should_return_created_201_status_code()
    {
        var command = new SignUp(Guid.Empty, "test-user@myspot.io", "test-user", "secret", "Test User", Role.User());
        
        var response = await Client.PostAsJsonAsync("users", command);
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        response.Headers.Location.ShouldNotBeNull();
    }

    [Test]
    public async Task post_sign_in_should_return_ok_200_status_code_and_jwt()
    {
        await CreateUserAsync(password);

        var command = new SignIn("test-user1@myspot.io", password);
        var response = await Client.PostAsJsonAsync("users/sign-in", command);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var jwt = await response.Content.ReadFromJsonAsync<JwtDto>();
        jwt.ShouldNotBeNull();
        jwt.AccessToken.ShouldNotBeNull();
    }
    
    [Test]
    public async Task get_users_me_should_return_ok_200_status_code_and_user()
    {
        var user = await CreateUserAsync(password);

        Authorize(user.Id, user.Role);
        var userDto = await Client.GetFromJsonAsync<UserDto>("users/me");

        userDto.ShouldNotBeNull();
        userDto.Id.ShouldBe(user.Id.Value);
    }

    private async Task<User> CreateUserAsync(string password)
    {
        var clock = new Clock();
        var passwordManager = new PasswordManager(new PasswordHasher<User>());

        var user = new User(Guid.NewGuid(), "test-user1@myspot.io", 
            "test-user1", passwordManager.Secure(password), "Test User1", Role.Admin(), clock.Current().Value.DateTime);

        await _testDatabase.DbContext.Users.AddAsync(user);
        await _testDatabase.DbContext.SaveChangesAsync();

        return user;
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        _userRepository = new TestUserRepository();
        services.AddSingleton(_userRepository);
    }

    public void Dispose()
    {
        _testDatabase?.Dispose();
    }
}
