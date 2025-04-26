using Domain.Aggregates.ServiceCategory.Values;

namespace Domain.Aggregates.Schedule.Contracts;

public interface IServiceChecker
{
    Task<bool> DoesServiceExistsAsync(ServiceId serviceId);
}