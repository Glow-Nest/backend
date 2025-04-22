namespace Domain.Aggregates.Schedule;

public class ScheduleBusinessHours
{
    public static TimeOnly OpeningHour { get; } = new(9, 0);
    public static TimeOnly ClosingHour { get; } = new(18, 0);

    public static bool IsWithinWorkingHours(TimeOnly time)
    {
        return time >= OpeningHour && time <= ClosingHour;
    }
}