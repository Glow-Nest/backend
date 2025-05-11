using Domain.Common.OperationResult;
using QueryContracts.Contracts;

namespace QueryContracts.Queries.Product;

public class GetProductByNameQuery
{
    public record Query(string ProductName) : IQuery<Result<Answer>>;
    public record Answer(string ProductId, string ProductName);
}