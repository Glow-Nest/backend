using Domain.Common.BaseClasses;

namespace Domain.Aggregates.ServiceCategory.Values;

public class ServiceId : ValueObject
{
    public Guid Value { get; private set; }

    protected ServiceId(Guid value)
    {
        Value = value;
    }

    public static ServiceId Create() => new(Guid.NewGuid());

    public static ServiceId FromGuid(Guid guidId) => new(guidId);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}