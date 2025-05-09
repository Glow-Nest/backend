using Domain.Aggregates.ServiceCategory.Values;
using Domain.Common.OperationResult;

namespace Application.AppEntry.Commands.ServiceCategory;

public class DeleteServiceCommand
{
    public CategoryId CategoryId { get; }
    public ServiceId ServiceId { get; }
    
    public DeleteServiceCommand(CategoryId categoryId, ServiceId serviceId)
    {
        CategoryId = categoryId;
        ServiceId = serviceId;
    }
    
    public static Result<DeleteServiceCommand> Create(string categoryId, string serviceId)
    {
        var categoryIdResult = CategoryId.FromGuid(Guid.Parse(categoryId));
        var serviceIdResult = ServiceId.FromGuid(Guid.Parse(serviceId));
        
        return Result<DeleteServiceCommand>.Success(new DeleteServiceCommand(categoryIdResult, serviceIdResult));
    }
}