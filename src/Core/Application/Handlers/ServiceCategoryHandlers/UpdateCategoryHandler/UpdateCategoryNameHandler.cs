using Application.AppEntry;
using Application.AppEntry.Commands.ServiceCategory.UpdateCategoryCommand;
using Domain.Aggregates.ServiceCategory;
using OperationResult;

namespace Application.Handlers.ServiceCategoryHandlers.UpdateCategoryHandler;

public class UpdateCategoryNameHandler : ICommandHandler<UpdateCategoryNameCommand>
{
    
    private readonly ICategoryRepository _categoryRepository;
    
    public UpdateCategoryNameHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public async Task<Result> HandleAsync(UpdateCategoryNameCommand command)
    {
        var findCategoryResult = await _categoryRepository.GetAsync(command.Id);
        
        if (!findCategoryResult.IsSuccess)
        {
            return findCategoryResult.ToNonGeneric();
        }

        var category = findCategoryResult.Data;
        var updateResult = category.UpdateCategoryName(command.Name);
        
        if (!updateResult.IsSuccess)
        {
            return updateResult;
        }
        
        return Result.Success();
    }
}