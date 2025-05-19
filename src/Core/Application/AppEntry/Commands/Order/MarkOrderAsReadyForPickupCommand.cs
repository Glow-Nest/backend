using Domain.Aggregates.Order;
using Domain.Aggregates.Order.Values;
using OperationResult;

namespace Application.AppEntry.Commands.Order;

public class MarkOrderAsReadyForPickupCommand
{
    internal OrderId OrderId { get; }
    
    private MarkOrderAsReadyForPickupCommand(OrderId orderId)
    {
        OrderId = orderId;
    }
    
    public static Result<MarkOrderAsReadyForPickupCommand> Create(string orderIdStr)
    {
        var parseBool = Guid.TryParse(orderIdStr, out var orderIdGuid);
        if (!parseBool)
        {
            return Result<MarkOrderAsReadyForPickupCommand>.Fail(OrderErrorMessage.EmptyOrderId());
        }
        
        var orderIdResult = OrderId.FromGuid(orderIdGuid);
        if (!orderIdResult.IsSuccess)
        {
            return Result<MarkOrderAsReadyForPickupCommand>.Fail(orderIdResult.Errors);
        }
        
        var command = new MarkOrderAsReadyForPickupCommand(orderIdResult.Data);
        return Result<MarkOrderAsReadyForPickupCommand>.Success(command);
    }
}