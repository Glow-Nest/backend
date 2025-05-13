using Application.AppEntry;
using Application.AppEntry.Commands.Client.UpdateClient;
using Domain.Aggregates.Client;
using OperationResult;

namespace Application.Handlers.ClientHandlers.UpdateClientHandler;

public class UpdatePasswordHandler : ICommandHandler<UpdatePasswordCommand>
{
    private readonly IClientRepository _clientRepository;
    
    public UpdatePasswordHandler(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }
    
    public async Task<Result> HandleAsync(UpdatePasswordCommand command)
    {
        var findClientResult = await _clientRepository.GetAsync(command.Id);
        
        if (!findClientResult.IsSuccess)
        {
            return findClientResult.ToNonGeneric();
        }

        var client = findClientResult.Data;
        var updateResult = client.UpdatePassword(command.NewPassword);
        
        if (!updateResult.IsSuccess)
        {
            return updateResult;
        }
        
        return Result.Success();
    }
}