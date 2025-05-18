using Application.Handlers.OrderHandlers;
using Domain.Aggregates.Order;
using OperationResult;

namespace Application.Interfaces;

public interface IStripePaymentGatewayService
{
    Task<Result<string>> CreateCheckoutSessionAsync(OrderCheckoutSessionDto order);
}