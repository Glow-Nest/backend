using Domain.Common;
using OperationResult;

namespace Application.AppEntry.Decorators;

public class TransactionDecorator : ICommandDispatcher
{
    private readonly ICommandDispatcher _inner;
    private readonly IUnitOfWork _unitOfWork;
    private bool _changesSaved;

    public TransactionDecorator(ICommandDispatcher inner, IUnitOfWork unitOfWork)
    {
        _inner = inner;
        _unitOfWork = unitOfWork;
        _changesSaved = false;
    }

    public async Task<Result<TResponse>> DispatchAsync<TCommand, TResponse>(TCommand command)
    {
        var dispatchResult = await _inner.DispatchAsync<TCommand, TResponse>(command);
        
        if (dispatchResult.IsSuccess && !_changesSaved)
        {
            Console.WriteLine("TransactionDecorator: Dispatching command was successful. " + command!.GetType().Name);
            await _unitOfWork.SaveChangesAsync();
            _changesSaved = true;
        }

        return dispatchResult;
    }
}
