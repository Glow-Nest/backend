using Application.AppEntry;
using Application.AppEntry.Commands.Service;
using Domain.Aggregates.Service;
using Domain.Common.OperationResult;

namespace Application.Handlers.ServiceHandlers;

public class CreateServiceHandler : ICommandHandler<CreateServiceCommand>
{
    
    private readonly IServiceRepository _serviceRepository;
    
    public CreateServiceHandler(IServiceRepository serviceRepository)
    {
        _serviceRepository = serviceRepository;
    }
    
    public async Task<Result> HandleAsync(CreateServiceCommand command)
    {
        var result = await Service.Create(command.name, command.description, command.price, command.duration, command.mediaUrls);
        
        if (!result.IsSuccess)
        {
            return result.ToNonGeneric();
        }

        var serviceAddResult = await _serviceRepository.AddAsync(result.Data);
        
        if (!serviceAddResult.IsSuccess)
        {
            return serviceAddResult;
        }

        return Result.Success();
    }
}