using Domain.Common.BaseClasses;

namespace Domain.Aggregates.Order.Values;

public class OrderStatus : ValueObject
{
    public enum OrderStatusEnum
    {
        Created,
        Paid,
        ReadyForPickup
    }

    public static readonly OrderStatus Created = new(OrderStatusEnum.Created);
    public static readonly OrderStatus Paid = new(OrderStatusEnum.Paid);
    public static readonly OrderStatus ReadyForPickup = new(OrderStatusEnum.ReadyForPickup);

    internal OrderStatusEnum Value { get; }

    private OrderStatus(OrderStatusEnum status)
    {
        Value = status;
    }

    public static OrderStatus From(OrderStatusEnum status) => status switch
    {
        OrderStatusEnum.Created => Created,
        OrderStatusEnum.Paid => Paid,
        OrderStatusEnum.ReadyForPickup => ReadyForPickup,
        _ => throw new ArgumentOutOfRangeException(nameof(status), "Unknown order status")
    };

    public override string ToString() => Value.ToString();

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}