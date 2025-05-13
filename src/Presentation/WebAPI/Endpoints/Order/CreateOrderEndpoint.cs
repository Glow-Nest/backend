using Application.AppEntry;
using Application.AppEntry.Commands.Order;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.Order;

public class CreateOrderEndpoint(ICommandDispatcher commandDispatcher) : ProtectedSharedWithRequestAndResponse<CreateOrderEndpoint.CreateOrderRequest, CreateOrderEndpoint.CreateOrderResponse>
{
    public record CreateOrderRequest([FromBody] string ClientId, [FromBody] string PickupDate,[FromBody] double TotalPrice,[FromBody] List<OrderItemDto> OrderItems);
    public record CreateOrderResponse(string OrderId);
    
    [HttpPost("order/create")]
    public override async Task<ActionResult<CreateOrderResponse>> HandleAsync([FromBody] CreateOrderRequest request)
    {
        var commandResult = CreateOrderCommand.Create(request.ClientId, request.PickupDate, request.TotalPrice, request.OrderItems);
        if (!commandResult.IsSuccess)
        {
            return BadRequest(commandResult.Errors);
        }
        
        var dispatchResult = await commandDispatcher.DispatchAsync<CreateOrderCommand, CreateOrderResponse>(commandResult.Data);
        if (!dispatchResult.IsSuccess)
        {
            return BadRequest(dispatchResult.Errors);
        }
        
        var orderId = dispatchResult.Data.OrderId;
        return Ok(new CreateOrderResponse(orderId));
        }
}