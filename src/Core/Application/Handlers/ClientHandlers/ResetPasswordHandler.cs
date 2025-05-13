using Application.AppEntry;
using Application.AppEntry.Commands.Client;
using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Contracts;
using Domain.Common.Contracts;
using OperationResult;

namespace Application.Handlers.ClientHandlers;

public class ResetPasswordHandler : ICommandHandler<ResetPasswordCommand, None>
{
    
    private readonly IClientRepository _clientRepository;

    public ResetPasswordHandler(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }
    
    public async Task<Result<None>> HandleAsync(ResetPasswordCommand command)
    {
        var clientResult = await _clientRepository.GetAsync(command.Email);
        
        if (!clientResult.IsSuccess)
        {
            return clientResult.ToNonGeneric().ToNone();
        }
        
        var client = clientResult.Data;
        
        var resetResult = client.ResetPassword(command.NewPassword);
        if (!resetResult.IsSuccess)
        {
            return resetResult.ToNone();
        }

        return Result<None>.Success(None.Value);
    }
}