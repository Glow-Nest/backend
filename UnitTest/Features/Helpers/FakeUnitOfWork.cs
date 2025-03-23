using Domain.Common;
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
}