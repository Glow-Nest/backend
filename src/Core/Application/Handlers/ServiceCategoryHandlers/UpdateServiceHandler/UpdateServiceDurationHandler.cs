using Application.AppEntry;
using Application.AppEntry.Commands.ServiceCategory.UpdateServiceCommand;
using Domain.Aggregates.ServiceCategory;
using Domain.Common.OperationResult;

namespace Application.Handlers.ServiceCategoryHandlers.UpdateServiceHandler;

public class UpdateServiceDurationHandler : ICommandHandler<UpdateServiceDurationCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    
    public UpdateServiceDurationHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public async Task<Result> HandleAsync(UpdateServiceDurationCommand command)
    {
        var findCategoryResult = await _categoryRepository.GetAsync(command.Id);
        
        if (!findCategoryResult.IsSuccess)
        {
            return findCategoryResult.ToNonGeneric();
        }

        var category = findCategoryResult.Data;
        var updateResult = category.UpdateServiceDuration(command.ServiceId, command.Duration);
        
        if (!updateResult.IsSuccess)
        {
            return updateResult;
        }
        
        return Result.Success();
    }
}