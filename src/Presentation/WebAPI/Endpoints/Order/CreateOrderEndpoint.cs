using Application.AppEntry;
using Application.AppEntry.Commands.Order;
using Domain.Aggregates.Order.Values;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.Order;

public class CreateOrderEndpoint(ICommandDispatcher commandDispatcher) : ProtectedSharedWithRequestAndResponse<CreateOrderEndpoint.CreateOrderRequest, CreateOrderEndpoint.CreateOrderResponse>
{
    public record CreateOrderRequest(string ClientId, double TotalPrice, List<OrderItemDto> OrderItems);
    public record CreateOrderResponse(string OrderId);
    
    [HttpPost("order/create")]
    public override async Task<ActionResult<CreateOrderResponse>> HandleAsync([FromBody] CreateOrderRequest request)
    {
        var commandResult = CreateOrderCommand.Create(request.ClientId, request.TotalPrice, request.OrderItems);
        if (!commandResult.IsSuccess)
        {
            return BadRequest(commandResult.Errors);
        }
        
        var dispatchResult = await commandDispatcher.DispatchAsync<CreateOrderCommand, OrderId>(commandResult.Data);
        if (!dispatchResult.IsSuccess)
        {
            return BadRequest(dispatchResult.Errors);
        }
        
        var orderId = dispatchResult.Data;
        return Ok(new CreateOrderResponse(orderId.ToString()));
    }
}