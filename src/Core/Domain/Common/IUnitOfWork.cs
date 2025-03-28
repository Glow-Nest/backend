using Domain.Common.OperationResult;

namespace Domain.Common;

public interface IUnitOfWork
{
    Task<Result> SaveChangesAsync();
}