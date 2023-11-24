using System.Diagnostics;
using Microsoft.Extensions.Logging;
using MySpot.Application.Abstractions;

namespace MySpot.Infrastructure.Logging.Decorators;

internal sealed class LoggingCommandHandlerDecorator<TCommand>
    : ICommandHandler<TCommand> where TCommand : class, ICommand
{
    private readonly ICommandHandler<TCommand> _commandHandler;
    private readonly ILogger<ICommandHandler<TCommand>> _logger;

    public LoggingCommandHandlerDecorator(
        ICommandHandler<TCommand> commandHandler,
        ILogger<ICommandHandler<TCommand>> logger)
    {
        _commandHandler = commandHandler;
        _logger = logger;
    }

    public async Task HandleAsync(TCommand command)
    {
        var type = typeof(TCommand).Name;
        var sw = new Stopwatch();
        
        _logger.LogInformation("Started handling a command: {CommandName}...", type);
        sw.Start();

        await _commandHandler.HandleAsync(command);

        sw.Stop();
        _logger.LogInformation("Completed handling a command: {CommandName} in {Elapsed}.", type, sw.Elapsed);
    }
}
