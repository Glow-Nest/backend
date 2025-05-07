using Application.AppEntry;
using Application.AppEntry.Commands.ServiceCategory;
using Domain.Aggregates.ServiceCategory;
using Domain.Common.OperationResult;

namespace Application.Handlers.ServiceCategoryHandlers;

public class UpdateServiceHandler : ICommandHandler<UpdateServiceCommand>
{
    
    private readonly ICategoryRepository _categoryRepository;
    
    public UpdateServiceHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public Task<Result> HandleAsync(UpdateServiceCommand command)
    {
        var findCategoryResult = _categoryRepository.GetAsync(command.id);
        
        if (!findCategoryResult.Result.IsSuccess)
        {
            return Task.FromResult(findCategoryResult.Result.ToNonGeneric());
        }

        var category = findCategoryResult.Result.Data;
        
        category.UpdateService(command.serviceId, command.name, command.price, command.duration);
        
        return Task.FromResult(Result.Success());
    }
}