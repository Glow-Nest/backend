using Domain.Aggregates.Schedule.Contracts;
using Domain.Aggregates.Service;
using Domain.Aggregates.Service.Values;

namespace Services.Contracts.Schedule;

public class ServiceChecker(IServiceRepository serviceRepository) : IServiceChecker
{
    private readonly IServiceRepository _serviceRepository = serviceRepository;

    public async Task<bool> DoesServiceExistsAsync(ServiceId serviceId)
    {
        var serviceResult = await _serviceRepository.GetAsync(serviceId);
        return serviceResult.IsSuccess;
    }
}