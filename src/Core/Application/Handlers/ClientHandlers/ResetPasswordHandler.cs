using Application.AppEntry;
using Application.AppEntry.Commands.Client;
using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Contracts;
using Domain.Common.Contracts;
using OperationResult;

namespace Application.Handlers.ClientHandlers;

public class ResetPasswordHandler : ICommandHandler<ResetPasswordCommand>
{
    
    private readonly IClientRepository _clientRepository;

    public ResetPasswordHandler(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }
    
    public async Task<Result> HandleAsync(ResetPasswordCommand command)
    {
        var clientResult = await _clientRepository.GetAsync(command.Email);
        
        if (!clientResult.IsSuccess)
        {
            return clientResult.ToNonGeneric();
        }
        
        var client = clientResult.Data;
        
        var resetResult = client.ResetPassword(command.NewPassword);
        if (!resetResult.IsSuccess)
        {
            return resetResult;
        }

        return Result.Success();
    }
}