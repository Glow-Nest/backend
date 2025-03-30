using Domain.Common.BaseClasses;

namespace Domain.Aggregates.Values;

public class SalonOwnerId : ValueObject
{
    internal Guid Value { get; }
    
    protected SalonOwnerId(Guid value)
    {
        Value = value;
    }
    
    public static SalonOwnerId Create() => new(Guid.NewGuid());
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}