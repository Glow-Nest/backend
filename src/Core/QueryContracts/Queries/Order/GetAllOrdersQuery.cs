using OperationResult;
using QueryContracts.Contracts;

namespace QueryContracts.Queries.Order;

public class GetAllOrdersQuery
{
    public record Query(string Status, int Page, int PageSize) : IQuery<Result<Answer>>;
    public record Answer(List<OrderResponseDto> OrderResponseDtos, int TotalCount);
    public record OrderResponseDto(string OrderId, string OrderDate, string PickupDate, string Status, string TotalPrice, string CustomerName, List<OrderItemResponseDto> OrderItems);
    public record OrderItemResponseDto(string ProductId, string ProductName, string Quantity, string PriceWhenOrdering);
}