using Domain.Common.BaseClasses;

namespace Domain.Aggregates.Client.Values;

public class ClientId : ValueObject
{
    public Guid Value { get; private set; }

    protected ClientId(Guid value)
    {
        Value = value;
    }

    public static ClientId Create() => new(Guid.NewGuid());

    public static ClientId FromGuid(Guid guidId) => new ClientId(guidId);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}