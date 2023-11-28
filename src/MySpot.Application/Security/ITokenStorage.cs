using MySpot.Application.DTO;

namespace MySpot.Application.Security;

public interface ITokenStorage
{
    void Set(JwtDto jwtDto);
    JwtDto Get();
}
