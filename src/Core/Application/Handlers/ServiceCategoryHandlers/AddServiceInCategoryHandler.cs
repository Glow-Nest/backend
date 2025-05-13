using Application.AppEntry;
using Application.AppEntry.Commands.ServiceCategory;
using Domain.Aggregates.ServiceCategory;
using OperationResult;

namespace Application.Handlers.ServiceCategoryHandlers;

public class AddServiceInCategoryHandler : ICommandHandler<AddServiceInCategoryCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    
    public AddServiceInCategoryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public async Task<Result> HandleAsync(AddServiceInCategoryCommand command)
    {
        var categoryResult =  await _categoryRepository.GetAsync(command.categoryId);
        
        if (!categoryResult.IsSuccess)
        {
            return Result.Fail(categoryResult.Errors);
        }
        
        var category = categoryResult.Data;
        var serviceResult = category.AddService(command.name, command.price, command.duration);
        
        if (!serviceResult.Result.IsSuccess)
        {
            return Result.Fail(serviceResult.Result.Errors);
        }

        if (!serviceResult.Result.IsSuccess)
        {
            return await Task.FromResult(Result.Fail(serviceResult.Result.Errors));
        }

        return await Task.FromResult(Result.Success());
        
    }
}