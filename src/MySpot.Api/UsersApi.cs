using Microsoft.Extensions.Options;
using MySpot.Application.Abstractions;
using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Application.Queries;
using MySpot.Infrastructure.DAL;

namespace MySpot.Api;

internal static class UsersApi
{
    private const string MeRoute = "me";

    public static WebApplication UseUserApi(this WebApplication app)
    {
        app.MapGet("api/users/me", async (HttpContext context, IQueryHandler<GetUser, UserDto> handler) => {
            var userDto = await handler.HandleAsync(new GetUser
            {
                UserId = Guid.Parse(context.User.Identity.Name)
            });

            if (userDto is null)
                return Results.NotFound();

            return Results.Ok(userDto);
        }).RequireAuthorization().WithName(MeRoute);

        app.MapPost("api/users", async (SignUp command, ICommandHandler<SignUp> handler) =>
        {
            command = command with { UserId = Guid.NewGuid() };
            await handler.HandleAsync(command);

            return Results.CreatedAtRoute(MeRoute);
        });

        return app;
    }
}
