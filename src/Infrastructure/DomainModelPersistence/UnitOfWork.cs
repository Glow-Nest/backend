using Domain.Common;
using Domain.Common.OperationResult;

namespace Repositories;

public class UnitOfWork : IUnitOfWork
{
    public async Task<Result> SaveChangesAsync()
    {
        Console.WriteLine("Unit of work saved changes.");
        return await Task.FromResult(Result.Success());
    }
}