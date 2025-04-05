using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Contracts;

namespace UnitTest.Features.Helpers;

public class FakeEmailUniqueChecker : IEmailUniqueChecker
{
    private readonly IClientRepository _clientRepository;
    
    public FakeEmailUniqueChecker(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }
    public async Task<bool> IsEmailUniqueAsync(string email)
    {
        var allClients = _clientRepository.GetAllAsync().Result.Data;
        
        allClients = allClients.Where(c => c.EmailAddress == email).ToList();

        return await Task.FromResult(!allClients.Any());
    }
}