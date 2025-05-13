using Microsoft.Extensions.DependencyInjection;
using OperationResult;

namespace Application.AppEntry.Dispatchers;

internal class CommandDispatcher(IServiceProvider serviceProvider) : ICommandDispatcher
{
    public Task<Result<TResponse>> DispatchAsync<TCommand, TResponse>(TCommand command)
    {
        var handler = serviceProvider.GetRequiredService<ICommandHandler<TCommand, TResponse>>();
        return handler.HandleAsync(command);
    }
}