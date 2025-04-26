using Domain.Aggregates.ServiceCategory.Values;
using Domain.Common.OperationResult;

namespace Domain.Aggregates.ServiceCategory.Entities;

public class Service 
{
    internal ServiceId ServiceId { get;}
    internal ServiceName Name { get; private set; }
    internal Price Price { get; private set; }
    internal TimeSpan Duration { get; private set; }
    
    private Service()
    {   
        // For EFC
    }

    public Service(ServiceId serviceId, ServiceName name, Price price, TimeSpan duration)
    {
        ServiceId = serviceId;
        Name = name;
        Price = price;
        Duration = duration;
    }

    public static async Task<Result<Service>> Create(ServiceName name, Price price, TimeSpan duration)
    {
        var serviceId = ServiceId.Create();
        var service = new Service(serviceId, name,price, duration);
        return Result<Service>.Success(service);
    }
}