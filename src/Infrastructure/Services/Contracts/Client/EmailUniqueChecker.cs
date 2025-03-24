using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Contracts;

namespace Services.Contracts.Client;

public class EmailUniqueChecker(IClientRepository clientRepository) : IEmailUniqueChecker
{
    private readonly IClientRepository _clientRepository = clientRepository;
    
    public async Task<bool> IsEmailUniqueAsync(string email)
    {
        var allClientsResult = await _clientRepository.GetAllAsync();
        
        var exists = allClientsResult.Data.Any(c => c.EmailAddress.Equals(email, StringComparison.OrdinalIgnoreCase));       
        
        return !exists;
    }
}