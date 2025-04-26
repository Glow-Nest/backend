using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Values;
using Domain.Aggregates.Schedule.Contracts;

namespace Services.Contracts.Schedule;

public class ClientChecker(IClientRepository clientRepository) : IClientChecker
{
    private readonly IClientRepository _clientRepository = clientRepository;

    public async Task<bool> DoesClientExistsAsync(ClientId id)
    {
        var client = await _clientRepository.GetAsync(id);
        return client != null;
    }
}