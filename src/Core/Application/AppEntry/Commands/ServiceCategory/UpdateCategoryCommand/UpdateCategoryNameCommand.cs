using Domain.Aggregates.ServiceCategory.Values;
using OperationResult;

namespace Application.AppEntry.Commands.ServiceCategory.UpdateCategoryCommand;

public class UpdateCategoryNameCommand : CategoryUpdateCommandBase
{
    
    internal CategoryName Name { get; }
    
    public UpdateCategoryNameCommand(CategoryId id, CategoryName name) : base(id)
    {
        Name = name;
    }
    
    public static Result<UpdateCategoryNameCommand> Create(string id, string name)
    {
        var idResult = CategoryId.FromGuid(Guid.Parse(id));
        
        var nameResult = CategoryName.Create(name);
        if (!nameResult.IsSuccess)
        {
            return Result<UpdateCategoryNameCommand>.Fail(nameResult.Errors);
        }
        
        return Result<UpdateCategoryNameCommand>.Success(new UpdateCategoryNameCommand(idResult, nameResult.Data));
    }
}