using Domain.Aggregates.Order.Values;
using Domain.Aggregates.Product.Values;
using Domain.Common.BaseClasses;
using Domain.Common.Values;
using OperationResult;

namespace Domain.Aggregates.Order.Entities;

public class OrderItem : Entity<OrderItemId>
{
    internal ProductId ProductId { get; }
    internal Quantity Quantity { get; }
    internal Price PriceWhenOrdering { get; }
    
    private OrderItem(OrderItemId id, ProductId productId, Quantity quantity, Price priceWhenOrdering) : base(id)
    {
        ProductId = productId;
        Quantity = quantity;
        PriceWhenOrdering = priceWhenOrdering;
    }
    
    public static Result<OrderItem> Create(ProductId productId, Quantity quantity, Price priceWhenOrdering)
    {
        var orderItemIdResult = OrderItemId.Create();
        if (!orderItemIdResult.IsSuccess)
        {
            return Result<OrderItem>.Fail(orderItemIdResult.Errors);
        }
        
        var orderItem = new OrderItem(orderItemIdResult.Data, productId, quantity, priceWhenOrdering);
        return Result<OrderItem>.Success(orderItem);
    }
}