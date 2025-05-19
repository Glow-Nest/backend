using Application.AppEntry;
using Application.AppEntry.Commands.Order;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.Order;

public class CreateCheckoutSessionEndpoint(ICommandDispatcher dispatcher) : ProtectedSharedWithRequestAndResponse<CreateCheckoutSessionEndpoint.Request, CreateCheckoutSessionEndpoint.Response>
{
    public new record Request([FromRoute] string OrderId);
    public new record Response(string SessionId);

    [HttpPost("order/stripe/checkout-session/{OrderId}")]
    public override async Task<ActionResult<Response>> HandleAsync([FromRoute] Request request)
    {
        var stripeCommandResult = CreateCheckoutSessionCommand.Create(request.OrderId);
        if (!stripeCommandResult.IsSuccess)
        {
            return BadRequest(stripeCommandResult.Errors);
        }
        
        var dispatchResult = await dispatcher.DispatchAsync<CreateCheckoutSessionCommand, string>(stripeCommandResult.Data);
        if (!dispatchResult.IsSuccess)
        {
            return BadRequest(dispatchResult.Errors);
        }
        
        var sessionId = dispatchResult.Data;
        
        Console.WriteLine(sessionId);
        
        return Ok(new Response(sessionId));
    }
}