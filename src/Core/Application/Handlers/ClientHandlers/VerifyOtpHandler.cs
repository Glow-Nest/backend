using Application.AppEntry;
using Application.AppEntry.Commands.Client;
using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Contracts;
using Domain.Common;
using Domain.Common.Contracts;
using OperationResult;

namespace Application.Handlers.ClientHandlers;

public class VerifyOtpHandler : ICommandHandler<VerifyOtpCommand, None>
{
    private readonly IClientRepository _clientRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    
    public VerifyOtpHandler(IClientRepository clientRepository, IDateTimeProvider dateTimeProvider)
    {
        _clientRepository = clientRepository;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<Result<None>> HandleAsync(VerifyOtpCommand command)
    {
        var clientResult = await _clientRepository.GetAsync(command.email);
        
        if (!clientResult.IsSuccess)
        {
            return clientResult.ToNonGeneric().ToNone();
        }
        
        var client = clientResult.Data;
        var result = client.VerifyOtp(command.otpCode, _dateTimeProvider);
        
        if (!result.IsSuccess)
        {
            return result.ToNone();
        }

        return Result<None>.Success(None.Value);
    }
}