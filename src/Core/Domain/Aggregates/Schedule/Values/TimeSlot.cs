using Domain.Aggregates.Appointment;
using Domain.Common;
using Domain.Common.BaseClasses;
using Domain.Common.OperationResult;

namespace Domain.Aggregates.Schedule.Values;

public class TimeSlot : ValueObject
{
    public TimeOnly Start { get; private set; }
    public TimeOnly End { get; private set; }

    private TimeSlot(TimeOnly start, TimeOnly end)
    {
        Start = start;
        End = end;
    }

    public static Result<TimeSlot> Create(TimeOnly start, TimeOnly end)
    {
        if (!IsValidInterval(start) || !IsValidInterval(end))
        {
            return Result<TimeSlot>.Fail(GenericErrorMessage.TimeMustBeOnHalfHour());
        }

        if (end <= start)
        {
            return Result<TimeSlot>.Fail(ScheduleErrorMessage.EndTimeStartError());
        }

        var timeSlot = new TimeSlot(start, end);
        return Result<TimeSlot>.Success(timeSlot);
    }

    private static bool IsValidInterval(TimeOnly time)
    {
        return time.Minute == 0 || time.Minute == 30;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Start;
        yield return End;
    }

    public bool Overlaps(TimeSlot other)
    {
        return Start < other.End && End > other.Start;
    }
}