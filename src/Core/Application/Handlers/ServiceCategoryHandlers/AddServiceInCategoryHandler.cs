using Application.AppEntry;
using Application.AppEntry.Commands.ServiceCategory;
using Domain.Aggregates.ServiceCategory;
using OperationResult;

namespace Application.Handlers.ServiceCategoryHandlers;

public class AddServiceInCategoryHandler : ICommandHandler<AddServiceInCategoryCommand, None>
{
    private readonly ICategoryRepository _categoryRepository;
    
    public AddServiceInCategoryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public async Task<Result<None>> HandleAsync(AddServiceInCategoryCommand command)
    {
        var categoryResult =  await _categoryRepository.GetAsync(command.categoryId);
        
        if (!categoryResult.IsSuccess)
        {
            return Result<None>.Fail(categoryResult.Errors);
        }
        
        var category = categoryResult.Data;
        var serviceResult = category.AddService(command.name, command.price, command.duration);
        
        if (!serviceResult.Result.IsSuccess)
        {
            return Result<None>.Fail(serviceResult.Result.Errors);
        }

        if (!serviceResult.Result.IsSuccess)
        {
            return Result<None>.Fail(serviceResult.Result.Errors);
        }

        return Result<None>.Success(None.Value);
    }
}