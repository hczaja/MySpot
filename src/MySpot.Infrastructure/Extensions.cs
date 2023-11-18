using Microsoft.Extensions.DependencyInjection;
using MySpot.Application.Services;
using MySpot.Core.Repositories;
using MySpot.Infrastructure.Repositories;
using MySpot.Infrastructure.Services;

namespace MySpot.Infrastructure;

public static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IClock, Clock>();
        services.AddSingleton<IWeeklyParkingSpotRepository, InMemoryWeeklyParkingSpotRepository>();
        return services;
    }
}
