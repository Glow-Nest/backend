using Domain.Aggregates.Appointment.Contracts;

namespace Services.Contracts.Appointment;

public class BlockedTimeChecker : IBlockedTimeChecker
{
    public Task<bool> IsBlockedTimeAsync(DateOnly bookingDate, TimeOnly bookingStartTime, TimeOnly bookingEndTime)
    {
        throw new NotImplementedException();
    }
}