using Domain.Aggregates.Order;
using Domain.Aggregates.Order.Values;
using OperationResult;

namespace Application.AppEntry.Commands.Order;

public class MarkOrderAsCancelledCommand
{
    internal OrderId OrderId { get; }
    
    protected MarkOrderAsCancelledCommand(OrderId orderId)
    {
        this.OrderId = orderId;
    }
    
    public static Result<MarkOrderAsCancelledCommand> Create(string orderIdStr)
    {
        var parseResult = Guid.TryParse(orderIdStr, out var orderIdGuid);
        if (!parseResult)
        {
            return Result<MarkOrderAsCancelledCommand>.Fail(OrderErrorMessage.EmptyOrderId());
        }

        var orderIdResult = OrderId.FromGuid(orderIdGuid);
        if (!orderIdResult.IsSuccess)
        {
            return Result<MarkOrderAsCancelledCommand>.Fail(orderIdResult.Errors);
        }
        
        var command = new MarkOrderAsCancelledCommand(orderIdResult.Data);
        return Result<MarkOrderAsCancelledCommand>.Success(command);
    }
}