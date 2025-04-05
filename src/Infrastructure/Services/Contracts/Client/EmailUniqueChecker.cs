using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Contracts;

namespace Services.Contracts.Client;

public class EmailUniqueChecker(IClientRepository clientRepository) : IEmailUniqueChecker
{
    private readonly IClientRepository _clientRepository = clientRepository;

    public async Task<bool> IsEmailUniqueAsync(string email)
    {
        var allClients = _clientRepository.GetAllAsync().Result.Data;
        
        Console.WriteLine("All Clients: " + allClients.Count);

        allClients = allClients.Where(c => c.EmailAddress == email).ToList();

        return await Task.FromResult(!allClients.Any());
    }
}