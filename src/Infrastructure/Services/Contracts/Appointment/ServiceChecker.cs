using Domain.Aggregates.Appointment.Contracts;
using Domain.Aggregates.ServiceCategory.Values;

namespace Services.Contracts.Appointment;

public class ServiceChecker : IServiceChecker
{
    public Task<bool> DoesServiceExistsAsync(ServiceId serviceId)
    {
        throw new NotImplementedException();
    }
}