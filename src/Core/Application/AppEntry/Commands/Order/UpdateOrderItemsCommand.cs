using Domain.Aggregates.Order;
using Domain.Aggregates.Order.Values;
using Domain.Aggregates.Product.Values;
using Domain.Common;
using Domain.Common.Values;
using OperationResult;

namespace Application.AppEntry.Commands.Order;

public record UpdateOrderDto(string ProductId, int Quantity, double PriceWhenOrdering);

public class UpdateOrderItemsCommand(List<Domain.Aggregates.Order.OrderItemDto> orderItems, OrderId orderId)
{
    internal List<Domain.Aggregates.Order.OrderItemDto> OrderItems { get; } = orderItems;
    internal OrderId OrderId { get; } = orderId;
    
    public static Result<UpdateOrderItemsCommand> Create(string orderIdStr, List<UpdateOrderDto> orderItems)
    {
        var listOfErrors = new List<Error>();
        
        // order id
        var guidParse = Guid.TryParse(orderIdStr, out var orderIdGuid);
        if (!guidParse)
        {
            listOfErrors.Add(GenericErrorMessage.ErrorParsingGuid());
        }
        
        var orderId = OrderId.FromGuid(orderIdGuid);
        if (!orderId.IsSuccess)
        {
            listOfErrors.Add(OrderErrorMessage.InvalidOrderId());
        }
        
        // convert updateOrderDto to orderItemDto
        var createOrderItemDtos = orderItems
            .Select(x =>
            {
                var productIdResult = ProductId.FromGuid(Guid.Parse(x.ProductId));
                var quantityResult = Quantity.Create(x.Quantity);
                var priceResult = Price.Create(x.PriceWhenOrdering);

                return new Domain.Aggregates.Order.OrderItemDto(
                    productIdResult,
                    quantityResult.Data,
                    priceResult.Data);
            })
            .ToList();


        if (listOfErrors.Any())
        {
            return Result<UpdateOrderItemsCommand>.Fail(listOfErrors);
        }
        
        var updateOrderItemsCommand = new UpdateOrderItemsCommand(createOrderItemDtos, orderId.Data);
        return Result<UpdateOrderItemsCommand>.Success(updateOrderItemsCommand);
    }
}