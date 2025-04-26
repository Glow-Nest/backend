using Domain.Aggregates.ServiceCategory.Values;

namespace Domain.Aggregates.Appointment.Contracts;

public interface IServiceChecker
{
    Task<bool> DoesServiceExistsAsync(ServiceId serviceId);
}