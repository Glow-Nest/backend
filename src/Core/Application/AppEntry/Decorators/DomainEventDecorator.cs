using Domain.Common;
using Domain.Common.OperationResult;

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

    public async Task<Result> DispatchAsync<TCommand>(TCommand command)
    {
        var dispatchResult = await _inner.DispatchAsync(command);
        
        if (dispatchResult.IsSuccess)
        {
            var domainEvents = await _unitOfWork.GetDomainEvents();
            await _domainEventDispatcher.DispatchAsync(domainEvents);
        }

        return dispatchResult;
    }
}