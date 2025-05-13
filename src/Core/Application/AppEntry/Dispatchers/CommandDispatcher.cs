using Microsoft.Extensions.DependencyInjection;
using OperationResult;

namespace Application.AppEntry.Dispatchers;

internal class CommandDispatcher(IServiceProvider serviceProvider) : ICommandDispatcher
{
    public Task<Result> DispatchAsync<TCommand>(TCommand command)
    {
        var handler = serviceProvider.GetRequiredService<ICommandHandler<TCommand>>();
        return handler.HandleAsync(command);
    }
}