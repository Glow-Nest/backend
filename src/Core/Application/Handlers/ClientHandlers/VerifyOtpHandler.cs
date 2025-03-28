using Application.AppEntry;
using Application.AppEntry.Commands.Client;
using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Contracts;
using Domain.Common;
using Domain.Common.OperationResult;

namespace Application.Handlers.ClientHandlers;

public class VerifyOtpHandler : ICommandHandler<VerifyOtpCommand>
{
    private readonly IClientRepository _clientRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUnitOfWork _unitOfWork;
    
    public VerifyOtpHandler(IClientRepository clientRepository, IDateTimeProvider dateTimeProvider, IUnitOfWork unitOfWork)
    {
        _clientRepository = clientRepository;
        _dateTimeProvider = dateTimeProvider;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(VerifyOtpCommand command)
    {
        var clientResult = await _clientRepository.GetAsync(command.email);
        
        if (!clientResult.IsSuccess)
        {
            return clientResult.ToNonGeneric();
        }
        
        var client = clientResult.Data;
        var result = client.VerifyOtp(command.otpCode, _dateTimeProvider);
        
        if (!result.IsSuccess)
        {
            return result;
        }

        return result;
    }
}