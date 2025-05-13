using Application.AppEntry;
using Application.AppEntry.Commands.Order;
using Domain.Aggregates.Client;
using Domain.Aggregates.Order;
using Domain.Aggregates.Order.Values;
using Domain.Aggregates.Product;
using Domain.Common.Contracts;
using OperationResult;

namespace Application.Handlers.OrderHandlers;

public class CreateOrderHandler(
    IOrderRepository orderRepository,
    IClientRepository clientRepository,
    IProductRepository productRepository,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<CreateOrderCommand, OrderId>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
    private readonly IClientRepository _clientRepository = clientRepository;
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<Result<OrderId>> HandleAsync(CreateOrderCommand command)
    {
        // 1. Validate Client existence
        var clientResult = await _clientRepository.GetAsync(command.ClientId);
        if (!clientResult.IsSuccess)
        {
            return Result<OrderId>.Fail(clientResult.Errors);
        }

        // 2. Validate existence of all products referenced in order items
        foreach (var orderItem in command.OrderItems)
        {
            var productResult = await _productRepository.GetAsync(orderItem.ProductId);
            if (!productResult.IsSuccess)
            {
                return Result<OrderId>.Fail(productResult.Errors);
            }
        }

        // 3. Create Order using validated client ID, pickup date, and order items
        var orderResult = Order.Create(
            command.ClientId,
            command.PickupDate,
            command.OrderItems,
            _dateTimeProvider);

        if (!orderResult.IsSuccess)
        {
            return Result<OrderId>.Fail(orderResult.Errors);
        }

        // 4. Save the order now or in a unit of work pattern
        var addResult = await _orderRepository.AddAsync(orderResult.Data);
        if (!addResult.IsSuccess)
        {
            return Result<OrderId>.Fail(addResult.Errors);
        }

        return Result<OrderId>.Success(orderResult.Data.OrderId);
    }
}