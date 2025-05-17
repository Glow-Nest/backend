using Domain.Aggregates.Client;
using EfcQueries.Models;
using OperationResult;
using QueryContracts.Contracts;
using QueryContracts.Queries.User;

namespace EfcQueries.Queries.User;

public class GetFullNameByUserIdQueryHandler(PostgresContext context) : IQueryHandler<GetFullNameByUserId.Query, Result<GetFullNameByUserId.Answer>>
{
    private readonly PostgresContext _context = context;
    
    public Task<Result<GetFullNameByUserId.Answer>> HandleAsync(GetFullNameByUserId.Query query)
    {
        var user = _context.Clients
            .Where(user => user.ClientId.ToString() == query.ClientId)
            .Select(user => new GetFullNameByUserId.Answer(
                user.ClientId.ToString(),
                user.FirstName,
                user.LastName))
            .FirstOrDefault();

        if (user == null)
        {
            return Task.FromResult(Result<GetFullNameByUserId.Answer>.Fail(ClientErrorMessage.ClientNotFound()));
        }
        
        return Task.FromResult(Result<GetFullNameByUserId.Answer>.Success(user));
    }
}