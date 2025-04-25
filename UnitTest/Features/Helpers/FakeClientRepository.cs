using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Values;
using Domain.Common.OperationResult;
using Domain.Common.Repositories;

namespace UnitTest.Features.Helpers;

public class FakeClientRepository : IClientRepository
{
    private List<Client>  _listOfClients = new();

    Task<Result> IGenericRepository<Client, ClientId>.RemoveAsync(ClientId id)
    {
        throw new NotImplementedException();
    }

    Task<Result> IClientRepository.RemoveAsync(ClientId clientId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> AddAsync(Client clientToAdd)
    {
        _listOfClients.Add(clientToAdd);
        return await Task.FromResult(Result.Success());
    }

    public Task<Result> AddRangeAsync(IEnumerable<Client> aggregates)
    {
        throw new NotImplementedException();
    }

    public Task<Result<Client>> GetAsync(ClientId clientId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<Client>> GetAsync(Email email)
    {
        var client = _listOfClients.FirstOrDefault(c => c.Email.Equals(email));
        if (client == null)
        {
            return await Task.FromResult(Result<Client>.Fail(ClientErrorMessage.ClientNotFound()));
        }

        return await Task.FromResult(Result<Client>.Success(client));
    }

    public async Task<Result<List<Client>>> GetAllAsync()
    {
        var result = await Task.FromResult(Result<List<Client>>.Success(_listOfClients));
        return result;
    }

    public Task<Result> RemoveAsync()
    {
        throw new NotImplementedException();
    }
}