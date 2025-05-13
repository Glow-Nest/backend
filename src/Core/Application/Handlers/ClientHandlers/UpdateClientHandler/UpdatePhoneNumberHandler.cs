using Application.AppEntry;
using Application.AppEntry.Commands.Client.UpdateClient;
using Domain.Aggregates.Client;
using OperationResult;

namespace Application.Handlers.ClientHandlers.UpdateClientHandler;

public class UpdatePhoneNumberHandler : ICommandHandler<UpdatePhoneNumberCommand>
{
    private readonly IClientRepository _clientRepository;
    
    public UpdatePhoneNumberHandler(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }
    public async Task<Result> HandleAsync(UpdatePhoneNumberCommand command)
    {
        var findClientResult = await _clientRepository.GetAsync(command.Id);
        
        if (!findClientResult.IsSuccess)
        {
            return findClientResult.ToNonGeneric();
        }

        var client = findClientResult.Data;
        var updateResult = client.UpdatePhoneNumber(command.PhoneNumber);
        
        if (!updateResult.IsSuccess)
        {
            return updateResult;
        }
        
        return Result.Success();
    }
}