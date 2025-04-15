using Application.AppEntry;
using Application.AppEntry.Commands.Client;
using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Contracts;
using Domain.Common.Contracts;
using Domain.Common.OperationResult;

namespace Application.Handlers.ClientHandlers;

public class InitiateResetPasswordHandler : ICommandHandler<InitiateResetPasswordCommand>
{
    
    private readonly IClientRepository _clientRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    
    public InitiateResetPasswordHandler(IClientRepository clientRepository, IDateTimeProvider dateTimeProvider)
    {
        _clientRepository = clientRepository;
        _dateTimeProvider = dateTimeProvider;
    }
    
    public async Task<Result> HandleAsync(InitiateResetPasswordCommand command)
    {
        var clientResult = await _clientRepository.GetAsync(command.email);
        
        if (!clientResult.IsSuccess)
        {
            return clientResult.ToNonGeneric();
        }
        
        var client = clientResult.Data;
        var result = client.CreatePasswordResetOtp(_dateTimeProvider);
        
        if (!result.IsSuccess)
        {
            return result.ToNonGeneric();
        }

        return Result.Success();
    }
}