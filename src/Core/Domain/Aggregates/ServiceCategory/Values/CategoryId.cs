using Domain.Common.BaseClasses;

namespace Domain.Aggregates.ServiceCategory.Values;

public class CategoryId : ValueObject
{
    public Guid Value { get; private set; }

    protected CategoryId(Guid value)
    {
        Value = value;
    }

    public static CategoryId Create() => new(Guid.NewGuid());

    public static CategoryId FromGuid(Guid guidId) => new(guidId);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}