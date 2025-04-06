using Domain.Common;
using Domain.Common.BaseClasses;
using Domain.Common.OperationResult;
using Domain.Common.Repositories;
using DomainModelPersistence.EfcConfigs;
using Microsoft.EntityFrameworkCore;

namespace DomainModelPersistence.Common;

public class RepositoryEfcBase<TAggr, TId> : IGenericRepository<TAggr, TId> where TAggr : AggregateRoot
{
    public static Error EntityNotFound() => new Error("Entity.NotFound", "Entity not found in the database");

    private readonly DomainModelContext _context;
    private readonly IUnitOfWork _unitOfWork;

    // Constructor where both context and unit of work are injected
    public RepositoryEfcBase(DomainModelContext context, IUnitOfWork unitOfWork)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public virtual async Task<Result> AddAsync(TAggr aggregate)
    {
        await _unitOfWork.Track(aggregate); // Track entity via UoW
        return Result.Success();
    }

    public virtual async Task<Result<TAggr>> GetAsync(TId id)
    {
        var root = await _context.Set<TAggr>().FindAsync(id);

        if (root is null)
        {
            return Result<TAggr>.Fail(EntityNotFound());
        }

        return Result<TAggr>.Success(root);
    }

    public virtual async Task<Result> RemoveAsync(TId id)
    {
        var root = await _context.Set<TAggr>().FindAsync(id);

        if (root is null)
        {
            return Result.Fail(EntityNotFound());
        }

        _context.Set<TAggr>().Remove(root);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }
}