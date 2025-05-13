using OperationResult;

namespace Application.AppEntry;

public interface ICommandHandler<TCommand, TResponse>
{
    Task<Result<TResponse>> HandleAsync(TCommand command);
}
