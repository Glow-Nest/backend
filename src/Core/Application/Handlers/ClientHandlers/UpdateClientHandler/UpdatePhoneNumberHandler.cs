using Application.AppEntry;
using Application.AppEntry.Commands.Client.UpdateClient;
using Domain.Aggregates.Client;
using OperationResult;

namespace Application.Handlers.ClientHandlers.UpdateClientHandler;

public class UpdatePhoneNumberHandler : ICommandHandler<UpdatePhoneNumberCommand, None>
{
    private readonly IClientRepository _clientRepository;
    
    public UpdatePhoneNumberHandler(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }
    public async Task<Result<None>> HandleAsync(UpdatePhoneNumberCommand command)
    {
        var findClientResult = await _clientRepository.GetAsync(command.Id);
        
        if (!findClientResult.IsSuccess)
        {
            return findClientResult.ToNonGeneric().ToNone();
        }

        var client = findClientResult.Data;
        var updateResult = client.UpdatePhoneNumber(command.PhoneNumber);
        
        if (!updateResult.IsSuccess)
        {
            return updateResult.ToNone();
        }
        
        return Result<None>.Success(None.Value);
    }
}