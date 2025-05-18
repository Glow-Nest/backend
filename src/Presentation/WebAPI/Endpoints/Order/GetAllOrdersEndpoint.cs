using Microsoft.AspNetCore.Mvc;
using QueryContracts.Queries.Order;
using QueryContracts.QueryDispatching;
using WebAPI.Endpoints.Common.Query;

namespace WebAPI.Endpoints.Order;

public class GetAllOrdersEndpoint(IQueryDispatcher queryDispatcher)
    : ProtectedQueryWithRequestAndResponse<GetAllOrdersEndpoint.Request, GetAllOrdersEndpoint.Response>
{
    public new record Request(string Status);
    public new record Response(List<OrderResponseDto> OrderResponseDtos);

    public record OrderResponseDto(
        string OrderId,
        string OrderDate,
        string PickupDate,
        string Status,
        string TotalPrice,
        string CustomerName,
        List<OrderItemResponseDto> OrderItems);

    public record OrderItemResponseDto(
        string ProductId,
        string ProductName,
        string Quantity,
        string PriceWhenOrdering);

    [HttpPost("orders")]
    public override async Task<ActionResult<Response>> HandleAsync([FromBody] Request request)
    {
        var query = new GetAllOrdersQuery.Query(request.Status);
        var result = await queryDispatcher.DispatchAsync(query);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }

        var orders = result.Data.OrderResponseDtos.Select(o => new OrderResponseDto(
            o.OrderId,
            o.OrderDate,
            o.PickupDate,
            o.Status,
            o.TotalPrice,
            o.CustomerName,
            o.OrderItems.Select(oi => new OrderItemResponseDto(
                oi.ProductId,
                oi.ProductName,
                oi.Quantity,
                oi.PriceWhenOrdering
            )).ToList()
        )).ToList();

        return Ok(new Response(orders));
    }
}
