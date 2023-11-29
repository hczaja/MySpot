
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MySpot.Application.DTO;
using MySpot.Application.Security;
using MySpot.Infrastructure;
using MySpot.Infrastructure.Auth;
using MySpot.Infrastructure.Services;

namespace MySpot.Tests.Integration.Controllers;

public class ControllerTests
{
    private readonly IAuthenticator _authenticator;

    protected HttpClient Client { get; }
    protected JwtDto Authorize(Guid userId, string role)
    {
        var jwt = _authenticator.CreateToken(userId, role);
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt.AccessToken);
    
        return jwt;
    }

    public ControllerTests(OptionsProvider optionsProvider)
    {
        var options = optionsProvider.Get<AuthOptions>("auth");
        _authenticator = new Authenticator(new OptionsWrapper<AuthOptions>(options), new Clock());

        var app = new MySpotTestApp(ConfigureServices);
        Client = app.Client;
    }

    protected virtual void ConfigureServices(IServiceCollection services) { }
}
