using Application.AppEntry;
using Application.AppEntry.Commands.Order;
using Application.Interfaces;
using Domain.Aggregates.Order;
using Domain.Aggregates.Product;
using OperationResult;

namespace Application.Handlers.OrderHandlers;

public record OrderCheckoutSessionDto(Guid OrderId, double TotalPrice, List<OrderItemDto> OrderItemDtos);

public record OrderItemDto(Guid ProductId, int Quantity, string ProductName, double Price);

public class CreateCheckoutSessionCommandHandler(
    IOrderRepository orderRepository,
    IProductRepository productRepository,
    IStripePaymentGatewayService stripePaymentGatewayService)
    : ICommandHandler<CreateCheckoutSessionCommand, string>
{
    private readonly IOrderRepository _orderRepository = orderRepository;
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IStripePaymentGatewayService _stripePaymentGatewayService = stripePaymentGatewayService;

    public async Task<Result<string>> HandleAsync(CreateCheckoutSessionCommand command)
    {
        // Get Order
        var orderResult = await _orderRepository.GetAsync(command.OrderId);
        if (!orderResult.IsSuccess)
        {
            return Result<string>.Fail(orderResult.Errors);
        }

        // Get Order Items
        var productIds = orderResult.Data.OrderItems.Select(item => item.ProductId).ToList();
        var products = await _productRepository.GetProductsByIdsAsync(productIds);
        if (!products.IsSuccess)
        {
            return Result<string>.Fail(products.Errors);
        }

        // Create Order Items DTO
        var orderItemDtos = products.Data.Select(product =>
        {
            var orderItemDto = new OrderItemDto
            (
                product.ProductId.Value,
                orderResult.Data.OrderItems.FirstOrDefault(item => item.ProductId.Value == product.ProductId.Value)
                    ?.Quantity.Value ?? 0,
                product.ProductName.Value,
                orderResult.Data.OrderItems.FirstOrDefault(item => item.ProductId.Value == product.ProductId.Value)
                    ?.PriceWhenOrdering.Value ?? 0
            );

            return orderItemDto;
        }).ToList();


        var order = orderResult.Data;

        var paymentIntentDto = new OrderCheckoutSessionDto(order.OrderId.Value, order.TotalPrice.Value, orderItemDtos);
        var stripeIntentResult = await _stripePaymentGatewayService.CreateCheckoutSessionAsync(paymentIntentDto);

        if (!stripeIntentResult.IsSuccess)
        {
            return Result<string>.Fail(stripeIntentResult.Errors);
        }

        return Result<string>.Success(stripeIntentResult.Data);
    }
}