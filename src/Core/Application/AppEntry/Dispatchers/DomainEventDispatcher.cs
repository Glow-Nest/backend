using Domain.Common;
using Microsoft.Extensions.DependencyInjection;
using OperationResult;

namespace Application.AppEntry.Dispatchers;

public class DomainEventDispatcher(IServiceProvider serviceProvider) : IDomainEventDispatcher
{
    public async Task<Result> DispatchAsync(List<IDomainEvent> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
        {
            var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
            dynamic handler = serviceProvider.GetRequiredService(handlerType);
            await handler.HandleAsync((dynamic)domainEvent);
        }
        
        return await Task.FromResult(Result.Success());
    }
}