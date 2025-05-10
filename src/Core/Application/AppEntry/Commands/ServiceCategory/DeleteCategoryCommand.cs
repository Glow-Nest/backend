using Application.AppEntry.Commands.Schedule;
using Domain.Aggregates.ServiceCategory.Values;
using Domain.Common;
using Domain.Common.OperationResult;

namespace Application.AppEntry.Commands.ServiceCategory;

public class DeleteCategoryCommand
{
    public CategoryId CategoryId { get; }
    
    public DeleteCategoryCommand(CategoryId categoryId)
    {
        CategoryId = categoryId;
    }
    
    public static Result<DeleteCategoryCommand> Create(string categoryId)
    {
        if (!Guid.TryParse(categoryId, out Guid id))
        {
            return Result<DeleteCategoryCommand>.Fail(GenericErrorMessage.ErrorParsingGuid());
        }
        
        return Result<DeleteCategoryCommand>.Success(new DeleteCategoryCommand(CategoryId.FromGuid(id)));
    }
}
