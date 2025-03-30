using Domain.Common.OperationResult;

namespace Application.AppEntry;

public interface ICommandHandler<T>
{
    Task<Result> HandleAsync(T command);
}

public interface ICommandHandler<TCommand, TResult>
{
    Task<Result<TResult>> HandleAsyncWithResult(TCommand command);
}