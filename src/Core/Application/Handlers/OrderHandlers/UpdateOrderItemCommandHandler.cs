using Application.AppEntry;
using Application.AppEntry.Commands.Order;
using Domain.Aggregates.Order;
using Domain.Aggregates.Order.Contracts;
using Domain.Aggregates.Order.Values;
using Domain.Common.Contracts;
using OperationResult;

namespace Application.Handlers.OrderHandlers;

public class UpdateOrderItemCommandHandler(
    IOrderRepository orderRepository,
    IDateTimeProvider dateTimeProvider,
    IProductChecker productChecker)
    : ICommandHandler<UpdateOrderItemsCommand, OrderId>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
    private readonly IProductChecker _productChecker = productChecker;

    public async Task<Result<OrderId>> HandleAsync(UpdateOrderItemsCommand command)
    {
        // check if order exists
        var orderResult = await _orderRepository.GetAsync(command.OrderId);
        if (!orderResult.IsSuccess)
        {
            return Result<OrderId>.Fail(orderResult.Errors);
        }

        var updateOrderItemsResult = await orderResult.Data.UpdateOrderItems(command.OrderItems, _productChecker);
        if (!updateOrderItemsResult.IsSuccess)
        {
            return Result<OrderId>.Fail(updateOrderItemsResult.Errors);
        }

        return Result<OrderId>.Success(updateOrderItemsResult.Data.OrderId);
    }
}