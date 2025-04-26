using Application.AppEntry;
using Application.AppEntry.Commands.ServiceCategory;
using Domain.Aggregates.ServiceCategory;
using Domain.Common.OperationResult;

namespace Application.Handlers.ServiceCategoryHandlers;

public class CreateCategoryHandler : ICommandHandler<CreateCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    
    public CreateCategoryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public async Task<Result> HandleAsync(CreateCategoryCommand command)
    {
        var result = await Category.Create(command.name, command.description, command.mediaUrls);
        if (!result.IsSuccess)
        {
            return result.ToNonGeneric();
        }

        var categoryAddResult = await _categoryRepository.AddAsync(result.Data);
        
        if (!categoryAddResult.IsSuccess)
        {
            return categoryAddResult;
        }
        
        return Result.Success();
    }
}