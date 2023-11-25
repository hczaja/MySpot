using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.DAL.Repositories;

internal sealed class PostgresUserRepository : IUserRepository
{
    private readonly DbSet<User> _users;

    public PostgresUserRepository(MySpotDbContext dbContext)
    {
        _users = dbContext.Users;
    }

    public async Task AddAsync(User user)
        => await _users.AddAsync(user);

    public Task<User> GetUserAsync(UserId id)
        => _users.SingleOrDefaultAsync(x => x.Id.Value == id.Value);

    public Task<User> GetUserByEmailAsync(string email)
        => _users.SingleOrDefaultAsync(x => x.Email == email);

    public Task<User> GetUserByUsernameAsync(string username)
        => _users.SingleOrDefaultAsync(x => x.Username == username);
}
