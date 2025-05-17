using Domain.Aggregates.Order;
using Domain.Aggregates.Order.Values;
using Domain.Common;
using OperationResult;

namespace Application.AppEntry.Commands.Order;

public class CreateCheckoutSessionCommand(OrderId orderId)
{
    internal OrderId OrderId { get; } = orderId;

    public static Result<CreateCheckoutSessionCommand> Create(string orderIdStr)
    {
        if (string.IsNullOrWhiteSpace(orderIdStr))
        {
            return Result<CreateCheckoutSessionCommand>.Fail(OrderErrorMessage.EmptyOrderId());
        }

        // parse order id
        var tryGuidParse = Guid.TryParse(orderIdStr, out var orderId);
        if (!tryGuidParse)
        {
            return Result<CreateCheckoutSessionCommand>.Fail(GenericErrorMessage.ErrorParsingGuid());
        }

        var parsedOrderId = OrderId.FromGuid(orderId);
        if (!parsedOrderId.IsSuccess)
        {
            return Result<CreateCheckoutSessionCommand>.Fail(parsedOrderId.Errors);
        }

        var paymentIntentCommand = new CreateCheckoutSessionCommand(parsedOrderId.Data);
        return Result<CreateCheckoutSessionCommand>.Success(paymentIntentCommand);
    }
}