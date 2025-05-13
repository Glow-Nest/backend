using Application.AppEntry;
using Application.AppEntry.Commands.ServiceCategory.UpdateServiceCommand;
using Domain.Aggregates.ServiceCategory;
using OperationResult;

namespace Application.Handlers.ServiceCategoryHandlers.UpdateServiceHandler;

public class UpdateServiceDurationHandler : ICommandHandler<UpdateServiceDurationCommand, None>
{
    private readonly ICategoryRepository _categoryRepository;
    
    public UpdateServiceDurationHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public async Task<Result<None>> HandleAsync(UpdateServiceDurationCommand command)
    {
        var findCategoryResult = await _categoryRepository.GetAsync(command.Id);
        
        if (!findCategoryResult.IsSuccess)
        {
            return findCategoryResult.ToNonGeneric().ToNone();
        }

        var category = findCategoryResult.Data;
        var updateResult = category.UpdateServiceDuration(command.ServiceId, command.Duration);
        
        if (!updateResult.IsSuccess)
        {
            return updateResult.ToNone();
        }
        
        return Result<None>.Success(None.Value);
    }
}