using Application.AppEntry;
using Application.AppEntry.Commands.Client;
using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Contracts;
using Domain.Common.OperationResult;

namespace Application.Handlers.ClientHandlers;

public class ResetPasswordHandler : ICommandHandler<ResetPasswordCommand>
{
    
    private readonly IClientRepository _clientRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public ResetPasswordHandler(IClientRepository clientRepository, IDateTimeProvider dateTimeProvider)
    {
        _clientRepository = clientRepository;
        _dateTimeProvider = dateTimeProvider;
    }
    
    public async Task<Result> HandleAsync(ResetPasswordCommand command)
    {
        var clientResult = await _clientRepository.GetAsync(command.Email);
        
        if (!clientResult.IsSuccess)
        {
            return clientResult.ToNonGeneric();
        }
        
        var client = clientResult.Data;
        
        var otpResult = client.VerifyOtp(command.OtpCode , _dateTimeProvider);
        if (!otpResult.IsSuccess)
        {
            return Result.Fail(otpResult.Errors);
        }
        
        var resetResult = client.ResetPassword(command.NewPassword);
        if (!resetResult.IsSuccess)
        {
            return resetResult;
        }

        return Result.Success();
    }
}