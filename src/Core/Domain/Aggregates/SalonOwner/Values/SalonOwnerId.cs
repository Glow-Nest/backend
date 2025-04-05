using Domain.Common.BaseClasses;

namespace Domain.Aggregates.SalonOwner.Values;

public class SalonOwnerId : ValueObject
{
    public Guid Value { get; private set; }
    
    protected SalonOwnerId(Guid value)
    {
        Value = value;
    }
    
    public static SalonOwnerId Create() => new(Guid.NewGuid());

    public static SalonOwnerId FromGuid(Guid id) => new SalonOwnerId(id);
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}