using MySpot.Application.DTO;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Security;

public interface IAuthenticator
{
    JwtDto CreateToken(UserId id, Role role);
}
