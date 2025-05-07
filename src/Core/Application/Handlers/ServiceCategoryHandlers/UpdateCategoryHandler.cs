using Application.AppEntry;
using Application.AppEntry.Commands.ServiceCategory;
using Domain.Aggregates.ServiceCategory;
using Domain.Common.OperationResult;

namespace Application.Handlers.ServiceCategoryHandlers;

public class UpdateCategoryHandler : ICommandHandler<UpdateCategoryCommand>
{
    
    private readonly ICategoryRepository _categoryRepository;
    
    public UpdateCategoryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public Task<Result> HandleAsync(UpdateCategoryCommand command)
    {
        var findCategoryResult = _categoryRepository.GetAsync(command.id);
        
        if (!findCategoryResult.Result.IsSuccess)
        {
            return Task.FromResult(findCategoryResult.Result.ToNonGeneric());
        }

        var category = findCategoryResult.Result.Data;
        
        category.UpdateCategory(command.name, command.description, command.mediaUrls);

        return Task.FromResult(Result.Success());
    }
}