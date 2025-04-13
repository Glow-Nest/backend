using Domain.Common.BaseClasses;
using Domain.Common.OperationResult;

namespace Domain.Common.Repositories;

public interface IGenericRepository<TAggr, in TId> where TAggr: AggregateRoot 
{
    Task<Result<TAggr>> GetAsync(TId id);
    Task<Result> RemoveAsync(TId id);
    Task<Result> AddAsync(TAggr aggregate);
}