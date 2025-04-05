using Domain.Common;
using Domain.Common.BaseClasses;
using Domain.Common.OperationResult;

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
        return new List<IDomainEvent>();
    }

    public async Task<Result> ClearDomainEvents()
    {
        return Result.Success();
    }

    public async Task<Result> Track(AggregateRoot aggregate)
    {
        return Result.Success();
    }
}