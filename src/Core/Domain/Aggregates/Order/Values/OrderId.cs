using Domain.Common.BaseClasses;
using OperationResult;

namespace Domain.Aggregates.Order.Values;

public class OrderId : ValueObject
{
    internal Guid Value { get; private set; }
    
    private OrderId(Guid value)
    {
        Value = value;
    }
    
    
    public static Result<OrderId> Create()
    {
        var orderId = Guid.NewGuid();
        return Result<OrderId>.Success(new OrderId(orderId));
    }
    
    public static Result<OrderId> FromGuid(Guid orderId)
    {
        if (orderId == Guid.Empty)
        {
            return Result<OrderId>.Fail(OrderErrorMessage.EmptyOrderId());
        }
        
        return Result<OrderId>.Success(new OrderId(orderId));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}