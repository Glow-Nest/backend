using Domain.Aggregates.ServiceCategory.Values;
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
        var categoryIdResult = CategoryId.FromGuid(Guid.Parse(categoryId));
        
        return Result<DeleteCategoryCommand>.Success(new DeleteCategoryCommand(categoryIdResult));
    }
}