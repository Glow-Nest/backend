using Application.AppEntry;
using Application.AppEntry.Commands.ServiceCategory.UpdateCategoryCommand;
using Domain.Aggregates.ServiceCategory;
using Domain.Common.OperationResult;

namespace Application.Handlers.ServiceCategoryHandlers.UpdateCategoryHandler;

public class UpdateCategoryDescriptionHandler : ICommandHandler<UpdateCategoryDescriptionCommand>
{
    
    private readonly ICategoryRepository _categoryRepository;
    
    public UpdateCategoryDescriptionHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public async Task<Result> HandleAsync(UpdateCategoryDescriptionCommand command)
    {
        var findCategoryResult = await _categoryRepository.GetAsync(command.Id);
        
        if (!findCategoryResult.IsSuccess)
        {
            return findCategoryResult.ToNonGeneric();
        }

        var category = findCategoryResult.Data;
        var updateResult = category.UpdateCategoryDescription(command.Description);
        
        if (!updateResult.IsSuccess)
        {
            return updateResult;
        }
        
        return Result.Success();
    }
}