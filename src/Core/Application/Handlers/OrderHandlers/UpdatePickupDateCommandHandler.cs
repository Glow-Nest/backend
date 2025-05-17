using Application.AppEntry;
using Application.AppEntry.Commands.Order;
using Domain.Aggregates.Order;
using Domain.Aggregates.Order.Values;
using Domain.Common.Contracts;
using OperationResult;

namespace Application.Handlers.OrderHandlers;

public class UpdatePickupDateCommandHandler(IOrderRepository orderRepository, IDateTimeProvider dateTimeProvider)
    : ICommandHandler<UpdatePickupDateCommand, OrderId>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

    public async Task<Result<OrderId>> HandleAsync(UpdatePickupDateCommand command)
    {
        // check if order exists
        var orderResult = await _orderRepository.GetAsync(command.OrderId);
        if (!orderResult.IsSuccess)
        {
            return Result<OrderId>.Fail(orderResult.Errors);
        }

        var updatePickupDateResult = orderResult.Data.UpdatePickupDate(command.PickupDate, _dateTimeProvider);
        if (!updatePickupDateResult.IsSuccess)
        {
            return Result<OrderId>.Fail(updatePickupDateResult.Errors);
        }

        return Result<OrderId>.Success(orderResult.Data.OrderId);
    }
}