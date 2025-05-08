using Application.AppEntry;
using Application.AppEntry.Commands.ServiceCategory.UpdateServiceCommand;
using Domain.Aggregates.ServiceCategory;
using Domain.Aggregates.ServiceCategory.Values;
using Domain.Common.OperationResult;

namespace Application.Handlers.ServiceCategoryHandlers.UpdateServiceHandler;

public class UpdateServiceNameHandler : ICommandHandler<UpdateServiceNameCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    
    public UpdateServiceNameHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public async Task<Result> HandleAsync(UpdateServiceNameCommand command)
    {
        var findCategoryResult = await _categoryRepository.GetAsync(command.Id);
        
        if (!findCategoryResult.IsSuccess)
        {
            return findCategoryResult.ToNonGeneric();
        }

        var category = findCategoryResult.Data;
        var updateResult = category.UpdateServiceName(command.ServiceId, command.Name);
        
        if (!updateResult.IsSuccess)
        {
            return updateResult;
        }
        
        return Result.Success();
    }
}