using Application.AppEntry;
using Application.AppEntry.Commands.Client;
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

    public CreateOtpHandler(IClientRepository clientRepository, IDateTimeProvider dateTimeProvider,
        IUnitOfWork unitOfWork)
    {
        _clientRepository = clientRepository;
        _dateTimeProvider = dateTimeProvider;
        _unitOfWork = unitOfWork;
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

        // TODO: send otp to client email
        Console.WriteLine(result.Data);

        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }
}