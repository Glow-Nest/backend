using OperationResult;
using QueryContracts.Contracts;

namespace QueryContracts.Queries.User;

public class GetFullNameByUserId
{
    public record Query(string ClientId) : IQuery<Result<Answer>>;
    public record Answer(string ClientId,string FirstName, string LastName);
    
}