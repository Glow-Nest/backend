using Application.AppEntry;
using Application.AppEntry.Commands.Client;
using Application.Common;
using Application.Interfaces;
using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Contracts;
using Domain.Common;
using Domain.Common.OperationResult;

namespace Application.Handlers.ClientHandlers;

internal class CreateOtpHandler : ICommandHandler<CreateOtpCommand>
{
    private readonly IClientRepository _clientRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailSender _emailSender;

    public CreateOtpHandler(IClientRepository clientRepository, IDateTimeProvider dateTimeProvider,
        IUnitOfWork unitOfWork, IEmailSender emailSender)
    {
        _clientRepository = clientRepository;
        _dateTimeProvider = dateTimeProvider;
        _unitOfWork = unitOfWork;
        _emailSender = emailSender;
    }

    public async Task<Result> HandleAsync(CreateOtpCommand command)
    {
        var clientResult = await _clientRepository.GetAsync(command.Email);

        if (!clientResult.IsSuccess)
        {
            return clientResult.ToNonGeneric();
        }

        var client = clientResult.Data;
        var result = client.CreateOtp(command.Purpose, _dateTimeProvider);

        if (!result.IsSuccess)
        {
            return result.ToNonGeneric();
        }

        var otpCode = result.Data;
        
        // TODO: send otp to client email
        var sendEmailResult = await _emailSender.SendEmailAsync(clientResult.Data, EmailPurpose.OtpCode, "OTP Code", otpCode.OtpCode.ToString());

        if (!sendEmailResult.IsSuccess)
        {
            return sendEmailResult;
        }
        
        Console.WriteLine(otpCode);

        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }
}
