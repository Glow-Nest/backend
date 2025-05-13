using Application.AppEntry;
using Application.AppEntry.Commands.ServiceCategory;
using Domain.Aggregates.ServiceCategory;
using OperationResult;

namespace Application.Handlers.ServiceCategoryHandlers;

public class DeleteServiceHandler : ICommandHandler<DeleteServiceCommand, None>
{
    private readonly ICategoryRepository _categoryRepository;
    
    public DeleteServiceHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public async Task<Result<None>> HandleAsync(DeleteServiceCommand command)
    {
        var categoryResult= await _categoryRepository.GetAsync(command.CategoryId);
        if (!categoryResult.IsSuccess)
        {
            return categoryResult.ToNonGeneric().ToNone();
        }
        var category = categoryResult.Data;
        var service = category.Services.FirstOrDefault(x => x.ServiceId == command.ServiceId);
        
        if (service == null)
        {
            return Result<None>.Fail(ServiceCategoryErrorMessage.ServiceNotFound());
        }
        await _categoryRepository.DeleteServiceAsync(command.CategoryId, command.ServiceId);
        return Result<None>.Success(None.Value);
    }
}