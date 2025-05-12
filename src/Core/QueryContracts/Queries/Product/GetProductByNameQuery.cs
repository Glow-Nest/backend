using Domain.Common.OperationResult;
using QueryContracts.Contracts;

namespace QueryContracts.Queries.Product;

public class GetProductByNameQuery
{
    public record Query(string ProductName) : IQuery<Result<List<Answer>>>;
    public record Answer(string ProductId, string ProductName, double Price,  string ImageUrl);
}