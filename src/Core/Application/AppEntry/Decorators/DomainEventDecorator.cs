using Domain.Common;
using OperationResult;

namespace Application.AppEntry.Decorators;

public class DomainEventDecorator : ICommandDispatcher
{
    private readonly ICommandDispatcher _inner;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    public DomainEventDecorator(ICommandDispatcher inner, IUnitOfWork unitOfWork, IDomainEventDispatcher domainEventDispatcher)
    {
        _inner = inner;
        _unitOfWork = unitOfWork;
        _domainEventDispatcher = domainEventDispatcher;
    }

    public async Task<Result<TResponse>> DispatchAsync<TCommand, TResponse>(TCommand command)
    {
        var dispatchResult = await _inner.DispatchAsync<TCommand, TResponse>(command);
        
        if (dispatchResult.IsSuccess)
        {
            var domainEvents = await _unitOfWork.GetDomainEvents();
            await _domainEventDispatcher.DispatchAsync(domainEvents);
        }

        return dispatchResult;
    }
}