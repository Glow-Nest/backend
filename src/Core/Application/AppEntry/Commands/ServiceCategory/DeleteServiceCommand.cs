using Domain.Aggregates.ServiceCategory.Values;
using Domain.Common;
using OperationResult;

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
        if (!Guid.TryParse(categoryId, out Guid id))
        {
            return Result<DeleteServiceCommand>.Fail(GenericErrorMessage.ErrorParsingGuid());
        }
        if (!Guid.TryParse(serviceId, out Guid serviceIdParsed))
        {
            return Result<DeleteServiceCommand>.Fail(GenericErrorMessage.ErrorParsingGuid());
        }
        
        return Result<DeleteServiceCommand>.Success(new DeleteServiceCommand(CategoryId.FromGuid(id), ServiceId.FromGuid(serviceIdParsed)));
    }
}