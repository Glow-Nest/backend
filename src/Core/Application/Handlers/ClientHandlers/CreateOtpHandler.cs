using Application.AppEntry;
using Application.AppEntry.Commands.Client;
using Application.Common;
using Application.Interfaces;
using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Contracts;
using Domain.Common;
using Domain.Common.Contracts;
using OperationResult;

namespace Application.Handlers.ClientHandlers;

internal class CreateOtpHandler : ICommandHandler<CreateOtpCommand, None>
{
    private readonly IClientRepository _clientRepository;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IEmailSender _emailSender;

    public CreateOtpHandler(IClientRepository clientRepository, IDateTimeProvider dateTimeProvider, IEmailSender emailSender)
    {
        _clientRepository = clientRepository;
        _dateTimeProvider = dateTimeProvider;
        _emailSender = emailSender;
    }

    public async Task<Result<None>> HandleAsync(CreateOtpCommand command)
    {
        var clientResult = await _clientRepository.GetAsync(command.Email);

        if (!clientResult.IsSuccess)
        {
            return clientResult.ToNonGeneric().ToNone();
        }

        var client = clientResult.Data;
        var result = client.CreateOtp(command.Purpose, _dateTimeProvider);

        if (!result.IsSuccess)
        {
            return result.ToNonGeneric().ToNone();
        }

        return Result<None>.Success(None.Value);
    }
}
