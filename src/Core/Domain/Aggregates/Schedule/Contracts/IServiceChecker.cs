using Domain.Aggregates.Service.Values;

namespace Domain.Aggregates.Schedule.Contracts;

public interface IServiceChecker
{
    Task<bool> DoesServiceExistsAsync(ServiceId serviceId);
}