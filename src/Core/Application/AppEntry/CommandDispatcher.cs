using System.Windows.Input;
using Domain.Common.OperationResult;
using Microsoft.Extensions.DependencyInjection;

namespace Application.AppEntry;

internal class CommandDispatcher(IServiceProvider serviceProvider) : ICommandDispatcher
{
    public Task<Result> DispatchAsync<TCommand>(TCommand command)
    {
        var handler = serviceProvider.GetRequiredService<ICommandHandler<TCommand>>();
        return handler.HandleAsync(command);
    }

    // public Task<Result<TResult>> DispatchAsync<TCommand, TResult>(TCommand command)
    // {
    //     var handler = serviceProvider.GetRequiredService<ICommandHandler<TCommand, TResult>>();
    //     return handler.HandleAsyncWithResult(command);
    // }
}