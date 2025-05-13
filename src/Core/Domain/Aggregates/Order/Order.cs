using Domain.Aggregates.Client.Values;
using Domain.Aggregates.Order.Entities;
using Domain.Aggregates.Order.Values;
using Domain.Aggregates.Product.Values;
using Domain.Common.BaseClasses;
using Domain.Common.Contracts;
using Domain.Common.Values;
using OperationResult;

namespace Domain.Aggregates.Order;

public class Order : AggregateRoot
{
    internal OrderId OrderId { get; }
    internal ClientId ClientId { get; }
    internal DateOnly PickupDate { get; }
    internal DateOnly OrderDate { get; }
    internal List<OrderItem> OrderItems { get; }
    internal Price TotalPrice { get; }
    internal OrderStatus OrderStatus { get; }
    internal PaymentStatus PaymentStatus { get; }

    private Order(OrderId orderId, ClientId clientId, DateOnly pickupDate, DateOnly orderDate, List<OrderItem> orderItems, Price totalPrice)
    {
        OrderId = orderId;
        ClientId = clientId;
        PickupDate = pickupDate;
        OrderDate = orderDate;
        OrderItems = orderItems;
        TotalPrice = totalPrice;

        OrderStatus = OrderStatus.Created;
        PaymentStatus = PaymentStatus.Pending;
    }

    public static Result<Order> Create(ClientId clientId, DateOnly pickupDate, List<OrderItem> orderItems, IDateTimeProvider dateTimeProvider)
    {
        var orderIdResult = OrderId.Create();
        if (!orderIdResult.IsSuccess)
        {
            return Result<Order>.Fail(orderIdResult.Errors);
        }

        var orderDate = DateOnly.FromDateTime(dateTimeProvider.GetNow());
        if (pickupDate < orderDate)
        {
            return Result<Order>.Fail(OrderErrorMessage.PickupDateInThePast());
        }

        var totalPrice = orderItems.Sum(item => item.PriceWhenOrdering.Value * item.Quantity.Value);
        var priceResult = Price.Create(totalPrice);

        if (!priceResult.IsSuccess)
        {
            return Result<Order>.Fail(priceResult.Errors);
        }

        var order = new Order(orderIdResult.Data, clientId, pickupDate, orderDate, orderItems, priceResult.Data);
        return Result<Order>.Success(order);
    }
}