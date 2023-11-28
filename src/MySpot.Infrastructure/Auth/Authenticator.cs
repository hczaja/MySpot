using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MySpot.Application.DTO;
using MySpot.Application.Security;
using MySpot.Core.Services;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.Auth;

public class Authenticator : IAuthenticator
{
    private readonly string _issuer;
    private readonly string _audience;
    private readonly TimeSpan _expiry;
    private readonly SigningCredentials _signingCredentials;
    private readonly IClock _clock;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new();

    public Authenticator(IOptions<AuthOptions> options, IClock clock)
    {
        _issuer = options.Value.Issuer;
        _audience = options.Value.Audience;
        _expiry = options.Value.Expiry ?? TimeSpan.FromHours(1);
        _signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SigningKey)),
            SecurityAlgorithms.HmacSha256);

        _clock = clock;
    }

    public JwtDto CreateToken(UserId id, Role role)
    {
        var now = _clock.Current().Value.DateTime;
        var expires = now.Add(_expiry);

        var claims = new List<Claim>()
        {
            new (JwtRegisteredClaimNames.Sub, id.ToString()),
            new (JwtRegisteredClaimNames.UniqueName, id.ToString()),
            new (ClaimTypes.Role, role)
        };
        
        var jwt = new JwtSecurityToken(_issuer, _audience, claims, now, expires, _signingCredentials);
        string accessToken = _jwtSecurityTokenHandler.WriteToken(jwt);
    
        return new JwtDto
        {
            AccessToken = accessToken
        };
    }
    
}