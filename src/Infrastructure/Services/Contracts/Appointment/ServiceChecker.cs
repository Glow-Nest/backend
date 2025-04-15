using Domain.Aggregates.Appointment.Contracts;
using Domain.Aggregates.Service.Values;

namespace Services.Contracts.Appointment;

public class ServiceChecker : IServiceChecker
{
    public Task<bool> DoesServiceExistsAsync(ServiceId serviceId)
    {
        throw new NotImplementedException();
    }
}