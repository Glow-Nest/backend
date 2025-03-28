using Domain.Common;
using Domain.Common.OperationResult;

namespace Application.AppEntry;

public interface IDomainEventDispatcher
{
    Task<Result> DispatchAsync(List<IDomainEvent> domainEvents);
}