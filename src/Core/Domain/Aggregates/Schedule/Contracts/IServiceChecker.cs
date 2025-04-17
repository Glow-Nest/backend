using Domain.Aggregates.Service.Values;

namespace Domain.Aggregates.Appointment.Contracts;

public interface IServiceChecker
{
    Task<bool> DoesServiceExistsAsync(ServiceId serviceId);
}