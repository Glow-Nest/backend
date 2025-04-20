using Domain.Common.BaseClasses;

namespace Domain.Aggregates.Schedule.Values.Appointment;

public class AppointmentId : ValueObject
{
    public Guid Value { get; private set; }

    protected AppointmentId(Guid value)
    {
        Value = value;
    }

    public static AppointmentId Create() => new(Guid.NewGuid());

    public static AppointmentId FromGuid(Guid guidId) => new AppointmentId(guidId);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}