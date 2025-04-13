using Application.AppEntry;
using Application.AppEntry.Commands.Client;
using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Contracts;
using Domain.Common;
using Domain.Common.OperationResult;

namespace Application.Handlers.ClientHandlers;

internal class CreateClientHandler : ICommandHandler<CreateClientCommand>
{
    private readonly IEmailUniqueChecker _emailUniqueChecker;
    private readonly IClientRepository _clientRepository;

    public CreateClientHandler(IEmailUniqueChecker emailUniqueChecker, IClientRepository clientRepository)
    {
        _emailUniqueChecker = emailUniqueChecker;
        _clientRepository = clientRepository;
    }

    public async Task<Result> HandleAsync(CreateClientCommand command)
    {
        var result = await Client.Create(command.fullName, command.email, command.password, command.phoneNumber, _emailUniqueChecker);

        if (!result.IsSuccess)
        {
            return result.ToNonGeneric();
        }

        var clientAddResult = await _clientRepository.AddAsync(result.Data);
        
        if (!clientAddResult.IsSuccess)
        {
            return clientAddResult;
        }

        return Result.Success();
    }
}