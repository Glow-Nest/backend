using Domain.Aggregates.Order;
using Domain.Aggregates.Order.Values;
using Domain.Common;
using OperationResult;

namespace Application.AppEntry.Commands.Order;

public class UpdatePickupDateCommand(DateOnly pickupDate, OrderId orderId)
{
    internal DateOnly PickupDate { get; } = pickupDate;
    internal OrderId OrderId { get; } = orderId;
    
    public static Result<UpdatePickupDateCommand> Create(string pickupDateStr, string orderIdStr)
    {
        var listOfErrors = new List<Error>();
        
        // pickup date
        var pickupDateParse = DateOnly.TryParse(pickupDateStr, out var pickupDate);
        if (!pickupDateParse)
        {
            listOfErrors.Add(GenericErrorMessage.ErrorParsingDate());
        }

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

        if (listOfErrors.Any())
        {
            return Result<UpdatePickupDateCommand>.Fail(listOfErrors);
        }
        
        var pickupDateCommand = new UpdatePickupDateCommand(pickupDate, orderId.Data);
        return Result<UpdatePickupDateCommand>.Success(pickupDateCommand);
    }
}