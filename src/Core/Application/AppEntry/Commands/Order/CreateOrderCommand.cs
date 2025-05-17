using System.Globalization;
using Domain.Aggregates.Client.Values;
using Domain.Aggregates.Order;
using Domain.Aggregates.Order.Entities;
using Domain.Aggregates.Order.Values;
using Domain.Aggregates.Product.Values;
using Domain.Common;
using Domain.Common.Values;
using OperationResult;

namespace Application.AppEntry.Commands.Order;

public record OrderItemDto(string ProductId, int Quantity, double PriceWhenOrdering);

public class CreateOrderCommand(Price totalPrice,  ClientId clientId, List<CreateOrderItemDto> orderItems, DateOnly pickupDate)
{
    internal ClientId ClientId { get; } = clientId;
    internal Price TotalPrice { get; } = totalPrice;
    internal List<CreateOrderItemDto> OrderItems { get; } = orderItems;
    internal DateOnly PickupDate { get; } = pickupDate;

    public static Result<CreateOrderCommand> Create(string clientIdStr, double totalPriceStr, string pickupDateStr, List<OrderItemDto> orderItemDtos)
    {
        var listOfErrors = new List<Error>();
        
        // client id
        var guidParse = Guid.TryParse(clientIdStr, out var clientIdGuid);
        if (!guidParse)
        {
            listOfErrors.Add(GenericErrorMessage.ErrorParsingGuid());
        }

        var clientId = ClientId.FromGuid(clientIdGuid);

        // total price
        var totalPriceParse = double.TryParse(totalPriceStr.ToString(CultureInfo.InvariantCulture), out var totalPrice);
        if (!totalPriceParse)
        {
            listOfErrors.Add(GenericErrorMessage.ErrorParsingPrice());
        }

        var totalPriceResult = Price.Create(totalPrice);
        if (!totalPriceResult.IsSuccess)
        {
            listOfErrors.AddRange(totalPriceResult.Errors);
        }
        
        // pickup date
        var pickupDateParse = DateOnly.TryParse(pickupDateStr, out var pickupDate);
        if (!pickupDateParse)
        {
            listOfErrors.Add(GenericErrorMessage.ErrorParsingDate());
        }

        // order items
        var orderItems = new List<CreateOrderItemDto>();
        foreach (var orderItemDto in orderItemDtos)
        {
            // product id
            var productIdParse = Guid.TryParse(orderItemDto.ProductId, out var productIdGuid);
            if (!productIdParse)
            {
                listOfErrors.Add(GenericErrorMessage.ErrorParsingGuid());
            }
            
            // quantity
            var quantityResult = Quantity.Create(orderItemDto.Quantity);
            if (!quantityResult.IsSuccess)
            {
                listOfErrors.AddRange(quantityResult.Errors);
            }
            
            // price when ordering
            var priceWhenOrderingResult = Price.Create(orderItemDto.PriceWhenOrdering);
            if (!priceWhenOrderingResult.IsSuccess)
            {
                listOfErrors.AddRange(priceWhenOrderingResult.Errors);
            }
            
            // create order item
            var productIdResult = ProductId.FromGuid(productIdGuid);
            var itemDto = new CreateOrderItemDto(productIdResult, quantityResult.Data, priceWhenOrderingResult.Data); 
            orderItems.Add(itemDto);
        }

        if (listOfErrors.Any())
        {
            return Result<CreateOrderCommand>.Fail(listOfErrors);
        }
        
        var command = new CreateOrderCommand(totalPriceResult.Data, clientId, orderItems, pickupDate);
        return Result<CreateOrderCommand>.Success(command);
    }
}