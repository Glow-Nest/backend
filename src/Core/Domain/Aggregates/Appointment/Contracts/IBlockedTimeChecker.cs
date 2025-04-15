namespace Domain.Aggregates.Appointment.Contracts;

public interface IBlockedTimeChecker
{
    Task<bool> IsBlockedTimeAsync(DateOnly bookingDate, TimeOnly bookingStartTime, TimeOnly bookingEndTime);
}