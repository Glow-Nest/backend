using Application.AppEntry;
using Application.AppEntry.Commands.ServiceCategory;
using Domain.Aggregates.ServiceCategory;
using Domain.Common.OperationResult;

namespace Application.Handlers.ServiceCategoryHandlers;

public class DeleteServiceHandler : ICommandHandler<DeleteServiceCommand>
{
    private readonly ICategoryRepository _categoryRepository;
    
    public DeleteServiceHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public async Task<Result> HandleAsync(DeleteServiceCommand command)
    {
        var categoryResult= await _categoryRepository.GetAsync(command.CategoryId);
        if (!categoryResult.IsSuccess)
        {
            return categoryResult.ToNonGeneric();
        }
        var category = categoryResult.Data;
        var service = category.Services.FirstOrDefault(x => x.ServiceId == command.ServiceId);
        
        if (service == null)
        {
            return Result.Fail(ServiceCategoryErrorMessage.ServiceNotFound());
        }
        await _categoryRepository.DeleteServiceAsync(command.CategoryId, command.ServiceId);
        return Result.Success();
    }
}