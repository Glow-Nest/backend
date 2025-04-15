using Domain.Aggregates.Service.Values;
using Domain.Common.BaseClasses;
using Domain.Common.OperationResult;

namespace Domain.Aggregates.Service;

public class Service : AggregateRoot
{
    internal ServiceId ServiceId { get;}


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