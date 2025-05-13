using Domain.Common.BaseClasses;

namespace Domain.Aggregates.Order.Values;

public enum PaymentStatusEnum
{
    Pending,
    Paid,
    Failed,
    Refunded
}

public sealed class PaymentStatus : ValueObject
{
    public static readonly PaymentStatus Pending = new(PaymentStatusEnum.Pending);
    public static readonly PaymentStatus Paid = new(PaymentStatusEnum.Paid);
    public static readonly PaymentStatus Failed = new(PaymentStatusEnum.Failed);
    public static readonly PaymentStatus Refunded = new(PaymentStatusEnum.Refunded);

    public PaymentStatusEnum Value { get; }

    private PaymentStatus(PaymentStatusEnum value)
    {
        Value = value;
    }

    public static PaymentStatus From(PaymentStatusEnum status) => status switch
    {
        PaymentStatusEnum.Pending => Pending,
        PaymentStatusEnum.Paid => Paid,
        PaymentStatusEnum.Failed => Failed,
        PaymentStatusEnum.Refunded => Refunded,
        _ => throw new ArgumentOutOfRangeException(nameof(status), "Invalid status")
    };

    public override string ToString() => Value.ToString();

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}