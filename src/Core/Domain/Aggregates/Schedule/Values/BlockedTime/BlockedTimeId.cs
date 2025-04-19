using Domain.Common.BaseClasses;

namespace Domain.Aggregates.Schedule.Values.BlockedTime;

public class BlockedTimeId : ValueObject
{
    public Guid Value { get; private set; }

    protected BlockedTimeId(Guid value)
    {
        Value = value;
    }

    public static BlockedTimeId Create() => new(Guid.NewGuid());

    public static BlockedTimeId FromGuid(Guid guidId) => new BlockedTimeId(guidId);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}