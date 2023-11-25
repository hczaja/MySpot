using MySpot.Core.Entities;
using MySpot.Core.ValueObjects;

namespace MySpot.Core.Repositories;

public interface IUserRepository
{
    Task<User> GetUserAsync(UserId id);
    Task<User> GetUserByEmailAsync(string email);
    Task<User> GetUserByUsernameAsync(string username);
    Task AddAsync(User user);
}
