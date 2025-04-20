using Domain.Aggregates.Service.Values;
using Domain.Common.BaseClasses;
using Domain.Common.OperationResult;

namespace Domain.Aggregates.Service;

public class Service : AggregateRoot
{
    internal ServiceId ServiceId { get;}
    internal Name Name { get; private set; }
    internal Description Description { get; private set; }
    internal Price Price { get; private set; }
    internal TimeSpan Duration { get; private set; }
    
    private Service()
    {
        // For EF
    }
    
    
    private Service(ServiceId serviceId)
    {
        ServiceId = serviceId;
    }

    public static async Task<Result<Service>> Create()
    {
        var serviceId = ServiceId.Create();
        var service = new Service(serviceId);
        return Result<Service>.Success(service);
    }
}