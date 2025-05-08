using Domain.Aggregates.ServiceCategory.Values;
using Domain.Common.OperationResult;

namespace Application.AppEntry.Commands.ServiceCategory.UpdateServiceCommand;

public class UpdateServiceDurationCommand : UpdateServiceCommandBase
{
    internal TimeSpan Duration { get; }
    
    protected UpdateServiceDurationCommand(CategoryId categoryId, ServiceId serviceId, TimeSpan duration) : base(categoryId, serviceId)
    {
        Duration = duration;
    }
    
    public static Result<UpdateServiceDurationCommand> Create(string categoryId, string serviceId, TimeSpan duration)
    {
        var categoryIdResult = CategoryId.FromGuid(Guid.Parse(categoryId));
        var serviceIdResult = ServiceId.FromGuid(Guid.Parse(serviceId));
        
        return Result<UpdateServiceDurationCommand>.Success(new UpdateServiceDurationCommand(categoryIdResult, serviceIdResult, duration));
    }
}