using Domain.Common.OperationResult;
using QueryContracts.Contracts;

namespace QueryContracts.Queries.Product;

public class GetAllProductsQuery
{
    public record Query(int Page, int PageSize) : IQuery<Result<Answer>>;
    public record Answer(List<ProductDto> Products, int TotalCount);
    public record ProductDto(string ProductId, string ProductName, double Price,  string ImageUrl);
}