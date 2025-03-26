using Application.AppEntry;
using Application.AppEntry.Commands.Client;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Endpoints.Common;

namespace WebAPI.Endpoints.Client;

public record VerifyOtpRequest(string Email, string OtpCode);

public class VerifyOtpEndpoint : CommandEndpoint.WithRequest<VerifyOtpRequest>.WithoutResponse
{
    [HttpPost("clients/otp/verify")]
    public override async Task<ActionResult> HandleAsync(VerifyOtpRequest request, ICommandDispatcher commandDispatcher)
    {
        var commandResult = VerifyOtpCommand.Create(request.Email, request.OtpCode);
        if (!commandResult.IsSuccess)
        {
            return BadRequest(commandResult.Errors);
        }

        var dispatchResult = await commandDispatcher.DispatchAsync(commandResult.Data);

        return dispatchResult.IsSuccess ? Ok() : BadRequest(dispatchResult.Errors);
    }
}