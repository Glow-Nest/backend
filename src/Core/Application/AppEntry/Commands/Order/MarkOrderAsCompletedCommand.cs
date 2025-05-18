using Domain.Aggregates.Order;
using Domain.Aggregates.Order.Values;
using OperationResult;

namespace Application.AppEntry.Commands.Order;

public class MarkOrderAsCompletedCommand(OrderId orderId)
{
    internal OrderId OrderId { get; } = orderId;

    public static Result<MarkOrderAsCompletedCommand> Create(string orderIdStr)
    {
        var parseResult = Guid.TryParse(orderIdStr, out var orderIdGuid);
        if (!parseResult)
        {
            return Result<MarkOrderAsCompletedCommand>.Fail(OrderErrorMessage.EmptyOrderId());
        }

        var orderIdResult = OrderId.FromGuid(orderIdGuid);
        if (!orderIdResult.IsSuccess)
        {
            return Result<MarkOrderAsCompletedCommand>.Fail(orderIdResult.Errors);
        }

        var command = new MarkOrderAsCompletedCommand(orderIdResult.Data);
        return Result<MarkOrderAsCompletedCommand>.Success(command);
    }
}