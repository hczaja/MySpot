using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using MySpot.Application.Abstractions;
using MySpot.Infrastructure.Logging.Decorators;
using Serilog;

namespace MySpot.Infrastructure.Logging;

public static class LoggingExtensions
{
    public static WebApplicationBuilder UseSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, configuration) => {
            configuration
                .WriteTo.Console()
                //.WriteTo.File("logs/logs.txt")
                .WriteTo.Seq("http://localhost:5341");
        });
    
        return builder;
    }


    public static IServiceCollection AddCustomLogging(this IServiceCollection services)
    {
        services.TryDecorate(typeof(ICommandHandler<>), typeof(LoggingCommandHandlerDecorator<>));
        return services;
    }
}
