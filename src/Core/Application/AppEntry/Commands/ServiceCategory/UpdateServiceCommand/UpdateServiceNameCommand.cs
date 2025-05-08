using Domain.Aggregates.ServiceCategory.Values;
using Domain.Common.OperationResult;

namespace Application.AppEntry.Commands.ServiceCategory.UpdateServiceCommand;

public class UpdateServiceNameCommand : UpdateServiceCommandBase
{
    
    internal ServiceName Name { get; }
    
    protected UpdateServiceNameCommand(CategoryId categoryId, ServiceId serviceId,ServiceName name) : base(categoryId, serviceId)
    {
        Name = name;
    }
    
    public static Result<UpdateServiceNameCommand> Create(string categoryId, string serviceId, string name)
    {
        var categoryIdResult = CategoryId.FromGuid(Guid.Parse(categoryId));
        var serviceIdResult = ServiceId.FromGuid(Guid.Parse(serviceId));
        
        var nameResult = ServiceName.Create(name);
        if (!nameResult.IsSuccess)
        {
            return Result<UpdateServiceNameCommand>.Fail(nameResult.Errors);
        }
        
        return Result<UpdateServiceNameCommand>.Success(new UpdateServiceNameCommand(categoryIdResult, serviceIdResult, nameResult.Data));
    }
}