using System.Windows.Input;
using OperationResult;

namespace Application.AppEntry;

public interface ICommandDispatcher
{
    Task<Result> DispatchAsync<TCommand>(TCommand command);
}