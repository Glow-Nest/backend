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
        await _categoryRepository.GetAsync(command.CategoryId);
        
        await _categoryRepository.DeleteServiceAsync(command.CategoryId, command.ServiceId);
        return Result.Success();
    }
}