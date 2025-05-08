using Domain.Aggregates.ServiceCategory.Values;
using Domain.Common.OperationResult;

namespace Application.AppEntry.Commands.ServiceCategory.UpdateCategoryCommand;

public class UpdateCategoryDescriptionCommand : CategoryUpdateCommandBase
{
    internal CategoryDescription Description { get; }
    
    public UpdateCategoryDescriptionCommand(CategoryId id, CategoryDescription description) : base(id)
    {
        Description = description;
    }
    
    public static Result<UpdateCategoryDescriptionCommand> Create(string id, string description)
    {
        var idResult = CategoryId.FromGuid(Guid.Parse(id));
        
        var descriptionResult = CategoryDescription.Create(description);
        if (!descriptionResult.IsSuccess)
        {
            return Result<UpdateCategoryDescriptionCommand>.Fail(descriptionResult.Errors);
        }
        
        return Result<UpdateCategoryDescriptionCommand>.Success(new UpdateCategoryDescriptionCommand(idResult, descriptionResult.Data));
    }
}