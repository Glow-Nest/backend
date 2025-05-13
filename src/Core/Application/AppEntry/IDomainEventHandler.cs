using OperationResult;

namespace Application.AppEntry;

public interface IDomainEventHandler<TEvent>
{
    Task<Result> HandleAsync(TEvent domainEvent);
}