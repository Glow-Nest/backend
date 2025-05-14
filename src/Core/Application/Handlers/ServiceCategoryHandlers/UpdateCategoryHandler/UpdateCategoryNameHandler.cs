using Application.AppEntry;
using Application.AppEntry.Commands.ServiceCategory.UpdateCategoryCommand;
using Domain.Aggregates.ServiceCategory;
using OperationResult;

namespace Application.Handlers.ServiceCategoryHandlers.UpdateCategoryHandler;

public class UpdateCategoryNameHandler : ICommandHandler<UpdateCategoryNameCommand, None>
{
    
    private readonly ICategoryRepository _categoryRepository;
    
    public UpdateCategoryNameHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public async Task<Result<None>> HandleAsync(UpdateCategoryNameCommand command)
    {
        var findCategoryResult = await _categoryRepository.GetAsync(command.Id);
        
        if (!findCategoryResult.IsSuccess)
        {
            return findCategoryResult.ToNonGeneric().ToNone();
        }

        var category = findCategoryResult.Data;
        var updateResult = category.UpdateCategoryName(command.Name);
        
        if (!updateResult.IsSuccess)
        {
            return updateResult.ToNone();
        }
        
        return Result<None>.Success(None.Value);
    }
}