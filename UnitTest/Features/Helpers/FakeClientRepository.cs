using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Values;
using Domain.Common.OperationResult;

namespace UnitTest.Features.Helpers;

public class FakeClientRepository : IClientRepository
{
    private List<Client>  _listOfClients = new();
    
    public async Task<Result> AddAsync(Client clientToAdd)
    {
        _listOfClients.Add(clientToAdd);
        return await Task.FromResult(Result.Success());
    }

    public Task<Result<Client>> GetAsync(ClientId clientId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<Client>> GetAsync(Email email)
    {
        var client = _listOfClients.FirstOrDefault(c => c.EmailValue.Equals(email));
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