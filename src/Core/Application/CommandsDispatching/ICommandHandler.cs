using Domain.Common.OperationResult;

namespace Application.CommandsDispatching;

public interface ICommandHandler<T>
{
    Task<Result> HandleAsync(T command);
}