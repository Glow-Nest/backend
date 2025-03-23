using Domain.Aggregates.Client;
using Domain.Common.OperationResult;

namespace UnitTest.Features.Helpers;

public class FakeClientRepository : IClientRepository
{
    public Task<Result> AddAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Result<Client>> GetAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Result> RemoveAsync()
    {
        throw new NotImplementedException();
    }
}