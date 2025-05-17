using Application.AppEntry;
using Application.AppEntry.Commands.Order;
using Domain.Aggregates.Client;
using Domain.Aggregates.Order;
using Domain.Aggregates.Order.Contracts;
using Domain.Aggregates.Order.Values;
using Domain.Aggregates.Product;
using Domain.Common.Contracts;
using OperationResult;

namespace Application.Handlers.OrderHandlers;

public class CreateOrderHandler(
    IOrderRepository orderRepository,
    IClientRepository clientRepository,
    IDateTimeProvider dateTimeProvider,
    IProductChecker productChecker)
    : ICommandHandler<CreateOrderCommand, OrderId>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IProductChecker _productChecker = productChecker ;

    public async Task<Result<OrderId>> HandleAsync(CreateOrderCommand command)
    {
        // 1. Validate Client existence
        var clientResult = await _clientRepository.GetAsync(command.ClientId);
        if (!clientResult.IsSuccess)
        {
            return Result<OrderId>.Fail(clientResult.Errors);
        }

        // 2. Create Order using validated client ID, pickup date, and order items
        var orderResult = await Order.Create(
            command.ClientId,
            command.OrderItems,
            command.PickupDate,
            _dateTimeProvider,
            _productChecker);

        if (!orderResult.IsSuccess)
        {
            return Result<OrderId>.Fail(orderResult.Errors);
        }

        // 3. Save the order now
        var addResult = await _orderRepository.AddAsync(orderResult.Data);
        if (!addResult.IsSuccess)
        {
            return Result<OrderId>.Fail(addResult.Errors);
        }

        return Result<OrderId>.Success(orderResult.Data.OrderId);
    }
}