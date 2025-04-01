using Domain.Common.OperationResult;

namespace Application.AppEntry;

public interface ICommandHandler<T>
{
    Task<Result> HandleAsync(T command);
}