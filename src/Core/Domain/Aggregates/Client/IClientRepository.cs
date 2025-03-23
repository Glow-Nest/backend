using Domain.Common.OperationResult;

namespace Domain.Aggregates.Client;

public interface IClientRepository
{
    Task<Result> AddAsync();
    Task<Result<Client>> GetAsync();
    Task<Result> RemoveAsync();
}