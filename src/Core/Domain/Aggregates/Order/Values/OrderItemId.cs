using Domain.Common.BaseClasses;
using OperationResult;

namespace Domain.Aggregates.Order.Values;

public class OrderItemId : ValueObject
{
    internal Guid Value { get; private set; }
    
    private OrderItemId(Guid value)
    {
        Value = value;
    }
    
    public static Result<OrderItemId> Create()
    {
        var orderItemId = Guid.NewGuid();
        return Result<OrderItemId>.Success(new OrderItemId(orderItemId));
    }
    
    public static Result<OrderItemId> FromGuid(Guid orderItemId)
    {
        if (orderItemId == Guid.Empty)
        {
            return Result<OrderItemId>.Fail(OrderErrorMessage.EmptyOrderItemId());
        }
        
        return Result<OrderItemId>.Success(new OrderItemId(orderItemId));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}