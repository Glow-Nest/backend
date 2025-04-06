using Domain.Common;
using Domain.Common.BaseClasses;
using Domain.Common.OperationResult;
using DomainModelPersistence.EfcConfigs;

namespace DomainModelPersistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly DomainModelContext _context;
    private bool _changesSaved = false;
    private Error ErrorInSaving= new Error("Error in saving changes",
        "An error occurred while saving changes to the database.");

    public UnitOfWork(DomainModelContext context)
    {
        _context = context;
    }
    
    public async Task<Result> SaveChangesAsync()
    {
        if (_changesSaved)
            return Result.Success();

        try
        {
            await _context.SaveChangesAsync();
            _changesSaved = true;
            return Result.Success();
        }
        catch (Exception)
        {
            return Result.Fail(ErrorInSaving);
        }
    }

    public async Task<Result> Track(AggregateRoot aggregate)
    {
        if (_context.ChangeTracker.Entries().All(entry => entry.Entity != aggregate))
        {
            _context.Add(aggregate); 
        }

        return await Task.FromResult(Result.Success());
    }

    public async Task<List<IDomainEvent>> GetDomainEvents()
    {
        return await Task.FromResult(_context.ChangeTracker
            .Entries<AggregateRoot>()
            .SelectMany(entry => entry.Entity.DomainEvents)
            .ToList());
    }

    public async Task<Result> ClearDomainEvents()
    {
        foreach (var entry in _context.ChangeTracker.Entries<AggregateRoot>())
        {
            entry.Entity.ClearDomainEvents();
        }

        return await Task.FromResult(Result.Success());
    }
}