using Domain.Common;
using Domain.Common.BaseClasses;
using Domain.Common.Repositories;
using Microsoft.EntityFrameworkCore;
using OperationResult;

namespace DomainModelPersistence.Repositories.Common;

public class RepositoryBase<TAggr, TId>(DbContext context) : IGenericRepository<TAggr, TId> where TAggr : AggregateRoot
{
    public virtual async Task<Result<TAggr>> GetAsync(TId id)
    {
        var root = await context.Set<TAggr>().FindAsync(id);

        if (root is null)
        {
            return Result<TAggr>.Fail(GenericErrorMessage.EntityNotFound());
        }

        return Result<TAggr>.Success(root);
    }

    public virtual async Task<Result> RemoveAsync(TId id)
    {
        var root = await context.Set<TAggr>().FindAsync(id);

        if (root is null)
        {
            return Result.Fail(GenericErrorMessage.EntityNotFound());
        }

        context.Set<TAggr>().Remove(root);

        return Result.Success();
    }

    public virtual async Task<Result> AddAsync(TAggr aggregate)
    {
        await context.Set<TAggr>().AddAsync(aggregate);
        return Result.Success();
    }

    public virtual async Task<Result> AddRangeAsync(IEnumerable<TAggr> aggregates)
    {
        await context.Set<TAggr>().AddRangeAsync(aggregates);
        return Result.Success();
    }
}