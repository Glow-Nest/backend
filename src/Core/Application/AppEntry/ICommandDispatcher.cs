using System.Windows.Input;
using OperationResult;

namespace Application.AppEntry;

public interface ICommandDispatcher
{
    Task<Result<TResponse>> DispatchAsync<TCommand, TResponse>(TCommand command);
}