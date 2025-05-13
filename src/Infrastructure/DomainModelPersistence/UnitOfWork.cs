using Domain.Common;
using Domain.Common.BaseClasses;
using DomainModelPersistence.EfcConfigs;
using OperationResult;

namespace DomainModelPersistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly DomainModelContext _context;
    private bool _changesSaved = false;

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
            return Result.Fail(GenericErrorMessage.ErrorInSaving());
        }
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