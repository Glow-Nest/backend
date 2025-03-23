using Application.CommandsDispatching;
using Application.CommandsDispatching.Commands.Client;
using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Contracts;
using Domain.Common;
using Domain.Common.OperationResult;

namespace Application.Handlers.ClientHandlers;

public class CreateClientHandler : ICommandHandler<CreateClientCommand>
{
    private readonly IEmailUniqueChecker _emailUniqueChecker;
    private readonly IClientRepository _clientRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateClientHandler(IEmailUniqueChecker emailUniqueChecker, IClientRepository clientRepository, IUnitOfWork unitOfWork)
    {
        _emailUniqueChecker = emailUniqueChecker;
        _clientRepository = clientRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> HandleAsync(CreateClientCommand command)
    {
        var result = await Client.Create(command.fullName, command.email, command.password, command.phoneNumber, _emailUniqueChecker);

        if (!result.IsSuccess)
        {
            return result.ToNonGeneric();
        }

        var clientAddResult = await _clientRepository.AddAsync(result.Data);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
}