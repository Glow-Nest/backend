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
    public Task<bool> IsEmailUniqueAsync(string email)
    {
        throw new NotImplementedException();
    }
}