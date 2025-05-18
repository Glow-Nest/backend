using Application.AppEntry;
using Application.AppEntry.Commands.Order;
using Microsoft.AspNetCore.Mvc;
using OperationResult;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.Order;

public class MarkOrderAsReadyForPickupEndpoint(ICommandDispatcher commandDispatcher) : ProtectedOwnerWithRequestAndResponse<MarkOrderAsReadyForPickupEndpoint.Request, MarkOrderAsReadyForPickupEndpoint.Response>
{
    public new record Request([FromRoute] string OrderId);
    public new record Response();

    public override async Task<ActionResult<Response>> HandleAsync([FromRoute] Request request)
    {
        var result = MarkOrderAsReadyForPickupCommand.Create(request.OrderId);
        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }
        
        var dispatchResult = await commandDispatcher.DispatchAsync<MarkOrderAsReadyForPickupCommand, None>(result.Data);
        if (!dispatchResult.IsSuccess)
        {
            return BadRequest(dispatchResult.Errors);
        }
        
        return Ok(new Response());
    }
}