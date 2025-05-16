using Application.Handlers.OrderHandlers;
using Application.Interfaces;
using Domain.Aggregates.Order;
using Microsoft.Extensions.Options;
using OperationResult;
using Stripe;
using Stripe.Checkout;

namespace Services.Stripe;

public class StripePaymentGateway : IStripePaymentGatewayService
{
    private readonly StripeSettings _settings;

    public StripePaymentGateway(IOptions<StripeSettings> options)
    {
        _settings = options.Value;
        StripeConfiguration.ApiKey = _settings.SecretKey;
    }

    public async Task<Result<string>> CreateCheckoutSessionAsync(OrderCheckoutSessionDto order)
    {
        // TODO: for testing only. Need to change success url and cancel url too
        var domain = "http://localhost:3000";
            
        var lineItems = order.OrderItemDtos.Select(item => new SessionLineItemOptions
        {
            Quantity = item.Quantity,
            PriceData = new SessionLineItemPriceDataOptions
            {
                Currency = "DKK",
                ProductData = new SessionLineItemPriceDataProductDataOptions
                {
                    Name = item.ProductName,
                },
                UnitAmountDecimal = (long)(item.Price * 100),
            },
        }).ToList();

        var options = new SessionCreateOptions()
        {
            PaymentMethodTypes = ["card"],
            Mode = "payment",
            LineItems = lineItems,
            SuccessUrl = $"{domain}/checkout/success",
            CancelUrl = $"{domain}/checkout",
            Metadata = new Dictionary<string, string>
            {
                { "OrderId", order.OrderId.ToString() }
            }
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options);

        return Result<string>.Success(session.Id);
    }
}