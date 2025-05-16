using Domain.Aggregates.Order;
using Domain.Aggregates.Order.Values;
using OperationResult;

namespace Application.AppEntry.Commands.Order;

public class MarkOrderAsPaidCommand
{
    internal OrderId OrderId { get; }
    internal string PaymentIntentId { get; }
    
    private MarkOrderAsPaidCommand(OrderId orderId, string paymentIntentId)
    {
        OrderId = orderId;
        PaymentIntentId = paymentIntentId;
    }
    
    public static Result<MarkOrderAsPaidCommand> Create(string? orderId, string? paymentIntentId)
    {
        if (string.IsNullOrWhiteSpace(orderId) || string.IsNullOrWhiteSpace(paymentIntentId))
        {
            return Result<MarkOrderAsPaidCommand>.Fail(OrderErrorMessage.StripePaymentIntentNotFound());
        }

        var idParse = Guid.TryParse(orderId, out var orderIdGuid);
        if (!idParse)
        {
            return Result<MarkOrderAsPaidCommand>.Fail(OrderErrorMessage.EmptyOrderId());
        }
        
        var orderIdResult = OrderId.FromGuid(orderIdGuid);
        if (!orderIdResult.IsSuccess)
        {
            return Result<MarkOrderAsPaidCommand>.Fail(orderIdResult.Errors);
        }

        return Result<MarkOrderAsPaidCommand>.Success(new MarkOrderAsPaidCommand(orderIdResult.Data, paymentIntentId));
    }
}