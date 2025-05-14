using Application.AppEntry;
using Application.AppEntry.Commands.ServiceCategory;
using Domain.Aggregates.ServiceCategory;
using OperationResult;

namespace Application.Handlers.ServiceCategoryHandlers;

public class CreateCategoryHandler : ICommandHandler<CreateCategoryCommand, None>
{
    private readonly ICategoryRepository _categoryRepository;
    
    public CreateCategoryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public async Task<Result<None>> HandleAsync(CreateCategoryCommand command)
    {
        var result = await Category.Create(command.name, command.description, command.mediaUrls);
        if (!result.IsSuccess)
        {
            return result.ToNonGeneric().ToNone();
        }

        var categoryAddResult = await _categoryRepository.AddAsync(result.Data);
        
        if (!categoryAddResult.IsSuccess)
        {
            return categoryAddResult.ToNone();
        }
        
        return Result<None>.Success(None.Value);
    }
}