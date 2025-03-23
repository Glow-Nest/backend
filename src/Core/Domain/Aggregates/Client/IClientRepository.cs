using Domain.Aggregates.Client.Values;
using Domain.Common.OperationResult;

namespace Domain.Aggregates.Client;

public interface IClientRepository
{
    Task<Result> AddAsync(Client client);
    Task<Result<Client>> GetAsync(ClientId clientId);
    Task<Result<List<Client>>> GetAllAsync();
    Task<Result> RemoveAsync();
}