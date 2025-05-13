using Application.AppEntry;
using Application.AppEntry.Commands.Client;
using Microsoft.AspNetCore.Mvc;
using OperationResult;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.Client.Otp;

public record VerifyOtpRequest(string Email, string OtpCode);

public class VerifyOtpEndpoint(ICommandDispatcher commandDispatcher) : PublicWithRequest<VerifyOtpRequest>
{
    [HttpPost("clients/otp/verify")]
    public override async Task<ActionResult> HandleAsync(VerifyOtpRequest request)
    {
        Console.WriteLine("Request received");
        
        var commandResult = VerifyOtpCommand.Create(request.Email, request.OtpCode);
        if (!commandResult.IsSuccess)
        {
            return BadRequest(commandResult.Errors);
        }

        var dispatchResult = await commandDispatcher.DispatchAsync<VerifyOtpCommand, None>(commandResult.Data);

        return dispatchResult.IsSuccess ? Ok() : BadRequest(dispatchResult.Errors);
    }
}