using Application.AppEntry;
using Application.AppEntry.Commands.Order;
using Domain.Aggregates.Order;
using OperationResult;

namespace Application.Handlers.OrderHandlers;

public class MarkOrderAsCompletedCommandHandler(IOrderRepository orderRepository)
    : ICommandHandler<MarkOrderAsCompletedCommand, None>
{
    private readonly IOrderRepository _orderRepository = orderRepository;

    public async Task<Result<None>> HandleAsync(MarkOrderAsCompletedCommand command)
    {
        var orderResult = await _orderRepository.GetAsync(command.OrderId);
        if (!orderResult.IsSuccess)
        {
            return Result<None>.Fail(orderResult.Errors);
        }
        
        var order = orderResult.Data;
        var completedResult = order.MarkAsCompleted();
        
        if (!completedResult.IsSuccess)
        {
            return Result<None>.Fail(completedResult.Errors);
        }
        
        return Result<None>.Success(None.Value);
    }
}