using Domain.Aggregates.Client.Values;
using Domain.Common.OperationResult;
using Domain.Common.Repositories;

namespace Domain.Aggregates.Client;

public interface IClientRepository : IGenericRepository<Client, ClientId>
{
    Task<Result> AddAsync(Client client);
    Task<Result<Client>> GetAsync(Email email);
    Task<Result<List<Client>>> GetAllAsync();
    Task<Result> RemoveAsync(ClientId clientId);
}