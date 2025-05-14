using Application.AppEntry;
using Application.AppEntry.Commands.ServiceCategory;
using Domain.Aggregates.ServiceCategory;
using OperationResult;

namespace Application.Handlers.ServiceCategoryHandlers;

public class DeleteCategoryHandler : ICommandHandler<DeleteCategoryCommand, None>
{
    private readonly ICategoryRepository _categoryRepository;
    
    public DeleteCategoryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public async Task<Result<None>> HandleAsync(DeleteCategoryCommand command)
    {
        var category = await _categoryRepository.GetAsync(command.CategoryId);
        
        if (!category.IsSuccess)
        {
            return category.ToNonGeneric().ToNone();
        }
        
        var deleteResult = await _categoryRepository.DeleteAsync(command.CategoryId);
        if (!deleteResult.IsSuccess)
        {
            return deleteResult.ToNone();
        }
        return Result<None>.Success(None.Value);
    }
}