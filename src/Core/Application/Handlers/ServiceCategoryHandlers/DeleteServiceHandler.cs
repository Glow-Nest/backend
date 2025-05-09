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
        
        await _categoryRepository.DeleteServiceAsync(command.CategoryId, command.ServiceId);
        return Result.Success();
    }
}