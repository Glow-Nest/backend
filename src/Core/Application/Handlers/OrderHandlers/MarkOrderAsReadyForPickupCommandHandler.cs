using Application.AppEntry;
using Application.AppEntry.Commands.Order;
using Domain.Aggregates.Order;
using OperationResult;

namespace Application.Handlers.OrderHandlers;

public class MarkOrderAsReadyForPickupCommandHandler(IOrderRepository orderRepository) : ICommandHandler<MarkOrderAsReadyForPickupCommand, None>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    public async Task<Result<None>> HandleAsync(MarkOrderAsReadyForPickupCommand command)
    {
        var orderResult = await _orderRepository.GetAsync(command.OrderId);
        if (!orderResult.IsSuccess)
        {
            return Result<None>.Fail(orderResult.Errors);
        }

        var order = orderResult.Data;
        order.MarkAsReadyForPickup();

        return Result<None>.Success(None.Value);
    }
}