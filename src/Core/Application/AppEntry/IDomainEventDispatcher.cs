using Domain.Common;
using OperationResult;

namespace Application.AppEntry;

public interface IDomainEventDispatcher
{
    Task<Result> DispatchAsync(List<IDomainEvent> domainEvents);
}