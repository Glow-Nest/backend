using Application.AppEntry;
using Application.AppEntry.Commands.Order;
using Microsoft.AspNetCore.Mvc;
using OperationResult;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.Order;

public class MarkOrderAsCancelledEndpoint(ICommandDispatcher commandDispatcher) : ProtectedOwnerWithRequestAndResponse<MarkOrderAsCancelledEndpoint.Request, MarkOrderAsCancelledEndpoint.Response>
{
    public new record Request([FromRoute] string OrderId);
    public new record Response();

    [HttpPost("orders/{OrderId}/mark-as-cancelled")]
    public override async Task<ActionResult<Response>> HandleAsync([FromRoute] Request request)
    {
        var command = MarkOrderAsCancelledCommand.Create(request.OrderId);
        if (!command.IsSuccess)
        {
            return BadRequest(command.Errors);
        }

        var dispatchResult = await commandDispatcher.DispatchAsync<MarkOrderAsCancelledCommand, None>(command.Data);
        if (!dispatchResult.IsSuccess)
        {
            return BadRequest(dispatchResult.Errors);
        }

        return Ok(new Response());
    }
}