using Application.AppEntry;
using Application.AppEntry.Commands.ServiceCategory;
using Domain.Aggregates.ServiceCategory;
using Domain.Common.OperationResult;

namespace Application.Handlers.ServiceCategoryHandlers;

public class DeleteCategoryHandler : ICommandHandler<DeleteCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    
    public DeleteCategoryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public async Task<Result> HandleAsync(DeleteCategoryCommand command)
    {
        var category = await _categoryRepository.GetAsync(command.CategoryId);
        
        var deleteResult = await _categoryRepository.DeleteAsync(category.Data);
        if (!deleteResult.IsSuccess)
        {
            return deleteResult;
        }
        return Result.Success();
    }
}