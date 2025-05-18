using Application.AppEntry;
using Application.AppEntry.Commands.Order;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.Order;

public class MarkOrderAsCompletedEndpoint(ICommandDispatcher commandDispatcher) : ProtectedOwnerWithRequestAndResponse<MarkOrderAsCompletedEndpoint.Request, MarkOrderAsCompletedEndpoint.Response>
{
    public new record Request([FromRoute] string OrderId);

    public new record Response();

    [HttpPost("orders/{OrderId}/mark-as-completed")]
    public override async Task<ActionResult<Response>> HandleAsync([FromRoute] Request request)
    {
        var command = MarkOrderAsCompletedCommand.Create(request.OrderId);
        if (!command.IsSuccess)
        {
            return BadRequest(command.Errors);
        }

        var dispatchResult = await commandDispatcher.DispatchAsync<MarkOrderAsCompletedCommand, string>(command.Data);
        if (!dispatchResult.IsSuccess)
        {
            return BadRequest(dispatchResult.Errors);
        }

        return Ok(new Response());
    }
}