using Domain.Common.BaseClasses;

namespace Domain.Aggregates.Schedule.Values;

public class ScheduleId : ValueObject
{
    public Guid Value { get; private set; }

    protected ScheduleId(Guid value)
    {
        Value = value;
    }

    public static ScheduleId Create() => new(Guid.NewGuid());

    public static ScheduleId FromGuid(Guid guidId) => new ScheduleId(guidId);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}