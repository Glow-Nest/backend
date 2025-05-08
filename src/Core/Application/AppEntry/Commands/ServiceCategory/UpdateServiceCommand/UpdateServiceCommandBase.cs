using Domain.Aggregates.ServiceCategory.Values;

namespace Application.AppEntry.Commands.ServiceCategory.UpdateServiceCommand;

public class UpdateServiceCommandBase
{
    internal CategoryId Id { get; }
    internal ServiceId ServiceId { get; }
    
    protected UpdateServiceCommandBase(CategoryId categoryId, ServiceId serviceId)
    {
        Id = categoryId;
        ServiceId = serviceId;
    }
}