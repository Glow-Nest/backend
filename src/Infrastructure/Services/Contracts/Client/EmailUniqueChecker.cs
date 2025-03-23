using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Contracts;

namespace Services.Contracts.Client;

public class EmailUniqueChecker(IClientRepository clientRepository) : IEmailUniqueChecker
{
    private IClientRepository _clientRepository = clientRepository;
    
    public Task<bool> IsEmailUniqueAsync(string email)
    {
        throw new NotImplementedException();
    }
}