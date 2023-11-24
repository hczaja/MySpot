using MySpot.Application.Abstractions;

namespace MySpot.Infrastructure.DAL.Decorators;

internal sealed class LoggingCommandHandlerDecorator<TCommand>
    : ICommandHandler<TCommand> where TCommand : class, ICommand
{
    ICommandHandler<TCommand> _commandHandler;

    public LoggingCommandHandlerDecorator(ICommandHandler<TCommand> commandHandler)
    {
        _commandHandler = commandHandler;
    }

    public async Task HandleAsync(TCommand command)
    {
        Console.WriteLine($"Processing command: {command.GetType().Name}");
        await _commandHandler.HandleAsync(command);
    }
}
