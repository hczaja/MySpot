using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Tests.Integration;

public class TestUserRepository : IUserRepository
{
    private readonly List<User> _users = new();

    public Task AddAsync(User user)
    {
        _users.Add(user);
        return Task.CompletedTask;
    }

    public Task<User> GetUserAsync(UserId id)
        => Task.FromResult(_users.SingleOrDefault(x => x.Id == id));

    public Task<User> GetUserByEmailAsync(string email)
        =>
         Task.FromResult(_users.SingleOrDefault(x => x.Email == email));
    public Task<User> GetUserByUsernameAsync(string username)
        => Task.FromResult(_users.SingleOrDefault(x => x.Username == username));
}
