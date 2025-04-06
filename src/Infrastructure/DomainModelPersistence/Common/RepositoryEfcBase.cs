using Domain.Common;
using Domain.Common.BaseClasses;
using Domain.Common.OperationResult;
using Domain.Common.Repositories;
using DomainModelPersistence.EfcConfigs;
using Microsoft.EntityFrameworkCore;

namespace DomainModelPersistence.Common;

public class RepositoryBase<TAggr, TId>(DbContext context) : IGenericRepository<TAggr, TId> where TAggr : AggregateRoot
{
    public static Error EntityNotFound() => new Error("0", "Entity not found in the database");

    public virtual async Task<Result<TAggr>> GetAsync(TId id)
    {
        var root = await context.Set<TAggr>().FindAsync(id);

        if (root is null)
        {
            return Result<TAggr>.Fail(EntityNotFound());
        }

        return Result<TAggr>.Success(root);
    }

    public virtual async Task<Result> RemoveAsync(TId id)
    {
        var root = await context.Set<TAggr>().FindAsync(id);

        if (root is null)
        {
            return Result.Fail(EntityNotFound());
        }

        context.Set<TAggr>().Remove(root);

        return Result.Success();
    }

    public virtual async Task<Result> AddAsync(TAggr aggregate)
    {
        await context.Set<TAggr>().AddAsync(aggregate);
        return Result.Success();
    }
}