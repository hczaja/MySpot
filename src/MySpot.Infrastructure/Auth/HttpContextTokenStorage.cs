using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using MySpot.Application.DTO;
using MySpot.Application.Security;

namespace MySpot.Infrastructure.Auth;

public class HttpContextTokenStorage : ITokenStorage
{
    private const string Token = "jwt";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextTokenStorage(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public JwtDto Get()
    {
        if (_httpContextAccessor.HttpContext is null)
            return null;

        if (_httpContextAccessor.HttpContext.Items.TryGetValue(Token, out var jwt))
            return jwt as JwtDto;

        return null;
    }

    public void Set(JwtDto jwtDto) => _httpContextAccessor.HttpContext?.Items.TryAdd(Token, jwtDto);
}
