using Application.AppEntry;
using Application.AppEntry.Commands.Client;
using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Contracts;
using Domain.Aggregates.Client.Values;
using Domain.Common.Contracts;
using OperationResult;

namespace Application.Handlers.ClientHandlers;

public class InitiateResetPasswordHandler : ICommandHandler<InitiateResetPasswordCommand, None>
{
    
    private readonly IClientRepository _clientRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    
    public InitiateResetPasswordHandler(IClientRepository clientRepository, IDateTimeProvider dateTimeProvider)
    {
        _clientRepository = clientRepository;
        _dateTimeProvider = dateTimeProvider;
    }
    
    public async Task<Result<None>> HandleAsync(InitiateResetPasswordCommand command)
    {
        var clientResult = await _clientRepository.GetAsync(command.email);
        
        if (!clientResult.IsSuccess)
        {
            return clientResult.ToNonGeneric().ToNone();
        }
        
        var client = clientResult.Data;
        var result = client.CreateOtp(Purpose.PasswordReset,_dateTimeProvider);
        
        if (!result.IsSuccess)
        {
            return result.ToNonGeneric().ToNone();
        }

        return Result<None>.Success(None.Value);
    }
}