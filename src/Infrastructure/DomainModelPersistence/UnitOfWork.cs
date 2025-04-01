using Domain.Common;
using Domain.Common.BaseClasses;
using Domain.Common.OperationResult;

namespace Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly List<AggregateRoot> _aggregates = new();
    private bool _changesSaved = false;

    public async Task<Result> SaveChangesAsync()
    {
        if (_changesSaved)
            return Result.Success();

        Console.WriteLine("Unit of work saved changes.");
        _changesSaved = true;
        return await Task.FromResult(Result.Success());
    }

    public async Task<Result> Track(AggregateRoot aggregate)
    {
        if (!_aggregates.Contains(aggregate))
            _aggregates.Add(aggregate);

        return Result.Success();
    }

    public async Task<List<IDomainEvent>> GetDomainEvents()
    {
        return await Task.FromResult(_aggregates
            .SelectMany(a => a.DomainEvents)
            .ToList());
    }

    public async Task<Result> ClearDomainEvents()
    {
        foreach (var aggregate in _aggregates)
            aggregate.ClearDomainEvents();

        return Result.Success();
    }
}