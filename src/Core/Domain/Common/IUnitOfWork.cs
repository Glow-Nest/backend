using Domain.Common.BaseClasses;
using OperationResult;

namespace Domain.Common;

public interface IUnitOfWork
{
    Task<Result> SaveChangesAsync();
    Task<List<IDomainEvent>> GetDomainEvents();
    
    Task<Result> ClearDomainEvents();
}