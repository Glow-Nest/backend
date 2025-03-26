using Application.AppEntry;
using Application.AppEntry.Commands.Client;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Endpoints.Common;

namespace WebAPI.Endpoints.Client;

public record CreateOtpRequest(string Email, string Purpose);

public class CreateOtpEndpoint : CommandEndpoint.WithRequest<CreateOtpRequest>.WithoutResponse
{
    [HttpPost("clients/otp/create")]
    public override async Task<ActionResult> HandleAsync(CreateOtpRequest request, ICommandDispatcher commandDispatcher)
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