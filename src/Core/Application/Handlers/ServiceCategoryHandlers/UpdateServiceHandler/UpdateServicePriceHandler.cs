using Application.AppEntry;
using Application.AppEntry.Commands.ServiceCategory;
using Application.AppEntry.Commands.ServiceCategory.UpdateServiceCommand;
using Domain.Aggregates.ServiceCategory;
using OperationResult;

namespace Application.Handlers.ServiceCategoryHandlers.UpdateServiceHandler;

public class UpdateServicePriceHandler : ICommandHandler<UpdateServicePriceCommand>
{
    
    private readonly ICategoryRepository _categoryRepository;
    
    public UpdateServicePriceHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public async Task<Result> HandleAsync(UpdateServicePriceCommand command)
    {
        var findCategoryResult = await _categoryRepository.GetAsync(command.Id);
        
        if (!findCategoryResult.IsSuccess)
        {
            return findCategoryResult.ToNonGeneric();
        }

        var category = findCategoryResult.Data;
        var updateResult = category.UpdateServicePrice(command.ServiceId, command.Price);
        
        if (!updateResult.IsSuccess)
        {
            return updateResult;
        }
        
        return Result.Success();
    }
}