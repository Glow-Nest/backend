using Application.AppEntry;
using Application.AppEntry.Commands.Client;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Endpoints.Common;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.Otp;

public record CreateOtpRequest(string Email, string Purpose);

public class CreateOtpEndpoint(ICommandDispatcher commandDispatcher) : PublicWithRequest<CreateOtpRequest>
{
    [HttpPost("clients/otp/create")]
    public override async Task<ActionResult> HandleAsync(CreateOtpRequest request)
    {
        var result = CreateOtpCommand.Create(request.Email, request.Purpose);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }

        var dispatchResult = await commandDispatcher.DispatchAsync(result.Data);
        return dispatchResult.IsSuccess ? Ok() : BadRequest(dispatchResult.Errors);
    }
}