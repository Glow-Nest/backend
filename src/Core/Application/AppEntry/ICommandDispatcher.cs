using System.Windows.Input;
using Domain.Common.OperationResult;

namespace Application.AppEntry;

public interface ICommandDispatcher
{
    Task<Result> DispatchAsync<TCommand>(TCommand command);
}