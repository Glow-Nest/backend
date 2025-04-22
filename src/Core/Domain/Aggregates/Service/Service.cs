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
    internal List<MediaUrl> MediaUrls { get; private set; }
    
    private Service()
    {
        // For EFC Core
    }

    public Service(ServiceId serviceId, Name name, Description description, Price price, TimeSpan duration, List<MediaUrl> mediaUrls)
    {
        ServiceId = serviceId;
        Name = name;
        Description = description;
        Price = price;
        Duration = duration;
        MediaUrls = mediaUrls;
    }

    public static async Task<Result<Service>> Create(Name name, Description description, Price price, TimeSpan duration, List<MediaUrl> mediaUrls)
    {
        var serviceId = ServiceId.Create();
        
        // validate if duration is 0 or 30 minutes
        if (duration.Minutes != 0 && duration.Minutes != 30)
        {
            return Result<Service>.Fail(ServiceErrorMessage.InvalidDuration());
        }
        
        var service = new Service(serviceId, name,description,price, duration,mediaUrls);
        return Result<Service>.Success(service);
    }
}