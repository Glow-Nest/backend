using Application.AppEntry;
using Application.AppEntry.Commands.Order;
using Domain.Aggregates.Order;
using OperationResult;

namespace Application.Handlers.OrderHandlers;

public class MarkOrderAsCancelledCommandHandler(IOrderRepository orderRepository) : ICommandHandler<MarkOrderAsCancelledCommand, None>
{
    private readonly IOrderRepository _orderRepository = orderRepository;

    public async Task<Result<None>> HandleAsync(MarkOrderAsCancelledCommand command)
    {
        var orderResult = await _orderRepository.GetAsync(command.OrderId);
        if (!orderResult.IsSuccess)
        {
            return Result<None>.Fail(orderResult.Errors);
        }
        
        var order = orderResult.Data;
        var cancelledResult = order.MarkAsCancelled();
        
        if (!cancelledResult.IsSuccess)
        {
            return Result<None>.Fail(cancelledResult.Errors);
        }
        
        return Result<None>.Success(None.Value);
    }
}