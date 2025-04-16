using Application.AppEntry;
using Application.AppEntry.Commands.Client;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Endpoints.Common;

namespace WebAPI.Endpoints.Client;

public record InitiatePasswordResetRequest(string Email);

public class InitiatePasswordResetEndpoint : CommandEndpoint.WithRequest<InitiatePasswordResetRequest>.WithoutResponse
{
    [HttpPost("clients/password/reset/initiate")]
    public override async Task<ActionResult> HandleAsync(InitiatePasswordResetRequest request, ICommandDispatcher commandDispatcher)
    {
        var result = InitiateResetPasswordCommand.Create(request.Email);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }

        var dispatchResult = await commandDispatcher.DispatchAsync(result.Data);
        return dispatchResult.IsSuccess ? Ok() : BadRequest(dispatchResult.Errors);
    }
}