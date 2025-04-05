using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Values;
using Domain.Common.OperationResult;

namespace Repositories.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly List<Client> _listOfClients = new();

    public async Task<Result> AddAsync(Client client)
    {
        _listOfClients.Add(client);

        Console.WriteLine($"Client with email {client.EmailAddress} added.");

        return await Task.FromResult(Result.Success());
    }

    public async Task<Result<Client>> GetAsync(ClientId clientId)
    {
        var client = _listOfClients.FirstOrDefault(c => c.ClientId.Equals(clientId));

        if (client == null)
        {
            return await Task.FromResult(Result<Client>.Fail(ClientErrorMessage.ClientNotFound()));
        }

        return await Task.FromResult(Result<Client>.Success(client));
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
        return await Task.FromResult(Result<List<Client>>.Success(_listOfClients.ToList()));
    }

    public async Task<Result> RemoveAsync()
    {
        _listOfClients.Clear();
        return await Task.FromResult(Result.Success());
    }
}