using Domain.Common;
using Domain.Common.BaseClasses;
using OperationResult;

namespace UnitTest.Features.Helpers;

public class FakeUnitOfWork : IUnitOfWork
{
    public bool SaveChangesCalled { get; private set; }

    public Task<Result> SaveChangesAsync()
    {
        SaveChangesCalled = true;
        return Task.FromResult(Result.Success());
    }

    public async Task<List<IDomainEvent>> GetDomainEvents()
    {
        return await Task.FromResult<List<IDomainEvent>>(new List<IDomainEvent>());
    }

    public async Task<Result> ClearDomainEvents()
    {
        return await Task.FromResult(Result.Success());
    }

    public async Task<Result> Track(AggregateRoot aggregate)
    {
        return await Task.FromResult(Result.Success());
    }
}