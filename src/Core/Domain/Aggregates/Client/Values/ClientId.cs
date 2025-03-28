using Domain.Common.BaseClasses;

namespace Domain.Aggregates.Client.Values;

public class ClientId : ValueObject
{
    internal Guid Value { get; private set; }

    protected ClientId(Guid value)
    {
        Value = value;
    }

    public static ClientId Create() => new(Guid.NewGuid());

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}