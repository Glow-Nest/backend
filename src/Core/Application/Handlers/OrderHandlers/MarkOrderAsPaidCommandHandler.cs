using Application.AppEntry;
using Application.AppEntry.Commands.Order;
using Domain.Aggregates.Order;
using OperationResult;

namespace Application.Handlers.OrderHandlers;

public class MarkOrderAsPaidCommandHandler(IOrderRepository orderRepository) : ICommandHandler<MarkOrderAsPaidCommand, None>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    
    public async Task<Result<None>> HandleAsync(MarkOrderAsPaidCommand command)
    {
        var order = await _orderRepository.GetAsync(command.OrderId);
        if (order == null)
        {
            return Result<None>.Fail(OrderErrorMessage.OrderNotFound());
        }

        var result = order.Data.MarkOrderAsPaid();
        if (!result.IsSuccess)
        {
            return Result<None>.Fail(result.Errors);
        }

        return Result<None>.Success(None.Value); 
    }
}