using Domain.Common.BaseClasses;
using OperationResult;

namespace Domain.Aggregates.Order.Values;

public class Quantity : ValueObject
{
    internal int Value { get; private set; }

    private Quantity(int value)
    {
        Value = value;
    }

    public static Result<Quantity> Create(int quantity)
    {
        if (quantity <= 0)
        {
            return Result<Quantity>.Fail(OrderErrorMessage.EmptyQuantity());
        }

        return Result<Quantity>.Success(new Quantity(quantity));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}