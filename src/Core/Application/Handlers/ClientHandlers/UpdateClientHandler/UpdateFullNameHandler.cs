using Application.AppEntry;
using Application.AppEntry.Commands.Client.UpdateClient;
using Domain.Aggregates.Client;
using Domain.Common.OperationResult;

namespace Application.Handlers.ClientHandlers.UpdateClientHandler;

public class UpdateFullNameHandler : ICommandHandler<UpdateFullNameCommand>
{
    private readonly IClientRepository _clientRepository;
    
    public UpdateFullNameHandler(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }
    public async Task<Result> HandleAsync(UpdateFullNameCommand command)
    {
        var findClientResult = await _clientRepository.GetAsync(command.Id);
        
        if (!findClientResult.IsSuccess)
        {
            return findClientResult.ToNonGeneric();
        }

        var client = findClientResult.Data;
        var updateResult = client.UpdateFullName(command.FullName);
        
        if (!updateResult.IsSuccess)
        {
            return updateResult;
        }
        
        return Result.Success();
    }
}