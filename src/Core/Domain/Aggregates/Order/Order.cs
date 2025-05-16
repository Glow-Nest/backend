using Domain.Aggregates.Client.Values;
using Domain.Aggregates.Order.Contracts;
using Domain.Aggregates.Order.Entities;
using Domain.Aggregates.Order.Values;
using Domain.Aggregates.Product;
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
    internal OrderStatus OrderStatus { get; private set; }
    internal PaymentStatus PaymentStatus { get; }

    // For EFC
    public Order()
    {
    }

    protected Order(OrderId orderId, ClientId clientId, DateOnly pickupDate, DateOnly orderDate, List<OrderItem> orderItems, Price totalPrice)
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

    public static async Task<Result<Order>> Create(ClientId clientId, List<OrderItem> orderItems,DateOnly pickupDate, IDateTimeProvider dateTimeProvider, IProductChecker productChecker)
    {
        // create order id
        var orderIdResult = OrderId.Create();
        if (!orderIdResult.IsSuccess)
        {
            return Result<Order>.Fail(orderIdResult.Errors);
        }
        
        // check if order items are empty
        if (orderItems.Count == 0)
        {
            return Result<Order>.Fail(OrderErrorMessage.NoOrderItems());
        }
        
        // check if product exists
        foreach (var orderItem in orderItems)
        {
            var productResult = await productChecker.DoesProductExist(orderItem.ProductId);
            if (!productResult)
            {
                return Result<Order>.Fail(ProductErrorMessage.ProductNotFound());
            }
        }
        
        // validate price in order items
        if (orderItems.Any(orderItem => orderItem.PriceWhenOrdering.Value < 0))
        {
            return Result<Order>.Fail(OrderErrorMessage.PriceCanNotBeNegative());
        }

        // validate pickup and order date
        var now = dateTimeProvider.GetNow();
        var today = DateOnly.FromDateTime(dateTimeProvider.GetNow());
        
        if (pickupDate < today)
        {
            return Result<Order>.Fail(OrderErrorMessage.PickupDateInThePast());
        }
        
        if (pickupDate == today && now.TimeOfDay > new TimeSpan(15, 0, 0))
        {
            return Result<Order>.Fail(OrderErrorMessage.PickupDateTooLateForToday());
        }

        var totalPrice = orderItems.Sum(item => item.PriceWhenOrdering.Value * item.Quantity.Value);
        var priceResult = Price.Create(totalPrice);

        if (!priceResult.IsSuccess)
        {
            return Result<Order>.Fail(priceResult.Errors);
        }

        var order = new Order(orderIdResult.Data, clientId, pickupDate, today, orderItems, priceResult.Data);
        return Result<Order>.Success(order);
    }

    public Result MarkOrderAsPaid()
    {
        if (OrderStatus != OrderStatus.Created)
        {
            return Result.Fail(OrderErrorMessage.OrderNotCreatedState());
        }
        
        OrderStatus = OrderStatus.Paid;
        return Result.Success();
    }

    public Result MarkOrderAsReadyForPickup()
    {
        if (OrderStatus != OrderStatus.Paid)
        {
            return Result.Fail(OrderErrorMessage.OrderNotPaidState());
        }
        
        OrderStatus = OrderStatus.ReadyForPickup;
        return Result.Success();
    }
    
    public Result MarkOrderAsCompleted()
    {
        if (OrderStatus != OrderStatus.ReadyForPickup)
        {
            return Result.Fail(OrderErrorMessage.OrderNotReadyForPickupState());
        }
        
        OrderStatus = OrderStatus.Completed;
        return Result.Success();
    }
    
    public Result MarkOrderAsCancelled()
    {
        if (OrderStatus is OrderStatus.Completed or OrderStatus.Cancelled)
        {
            return Result.Fail(OrderErrorMessage.OrderCannotBeCancelled());
        }

        OrderStatus = OrderStatus.Cancelled;
        return Result.Success();
    }

}