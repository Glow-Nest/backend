using OperationResult;
using QueryContracts.Contracts;

namespace QueryContracts.Queries.Product;

public class GetProductByIdQuery
{
    public record Query(string ProductId) : IQuery<Result<Answer>>;
    public record Answer(string ProductId, string ProductName, string Description, double Price, string ImageUrl, int InventoryCount);
}