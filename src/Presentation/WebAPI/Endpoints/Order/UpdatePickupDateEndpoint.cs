using Application.AppEntry;
using Application.AppEntry.Commands.Order;
using Domain.Aggregates.Order.Values;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.Order;

public class UpdatePickupDateEndpoint(ICommandDispatcher commandDispatcher) : PublicWithRequestAndResponse<UpdatePickupDateEndpoint.Request, UpdatePickupDateEndpoint.Response>
{
    public new record Request(string PickUpDate, string OrderId);
    public new record Response(string OrderId);
    
    [HttpPost("/order/update-pickup-date")]
    public override async Task<ActionResult<Response>> HandleAsync(Request request)
    {
        var command = UpdatePickupDateCommand.Create(request.PickUpDate, request.OrderId);
        if (!command.IsSuccess)
        {
            return BadRequest(command.Errors);
        }

        var result = await commandDispatcher.DispatchAsync<UpdatePickupDateCommand, OrderId>(command.Data);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }
        
        var response = new Response(result.Data.ToString());
        return Ok(response);
    }
}