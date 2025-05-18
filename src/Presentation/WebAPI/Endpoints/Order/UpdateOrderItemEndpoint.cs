using Application.AppEntry;
using Application.AppEntry.Commands.Order;
using Domain.Aggregates.Order;
using Domain.Aggregates.Order.Values;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.Order;

public class UpdateOrderItemEndpoint(ICommandDispatcher commandDispatcher) : PublicWithRequestAndResponse<UpdateOrderItemEndpoint.Request, UpdateOrderItemEndpoint.Response>
{
    public new record Request(string OrderId, List<UpdateOrderDto> OrderItems);
    public new record Response(string OrderId);

    [HttpPost("/order/update-order-items")]
    public override async Task<ActionResult<Response>> HandleAsync(Request request)
    {
        var command = UpdateOrderItemsCommand.Create(
            request.OrderId,
            request.OrderItems.Select(x => new UpdateOrderDto(x.ProductId, x.Quantity, x.PriceWhenOrdering)).ToList());

        if (!command.IsSuccess)
        {
            return BadRequest(command.Errors);
        }

        var result = await commandDispatcher.DispatchAsync<UpdateOrderItemsCommand, OrderId>(command.Data);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }

        var response = new Response(result.Data.ToString());
        return Ok(response);
    }
}