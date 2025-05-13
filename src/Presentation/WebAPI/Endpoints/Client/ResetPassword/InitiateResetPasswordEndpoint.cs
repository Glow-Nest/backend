using Application.AppEntry;
using Application.AppEntry.Commands.Client;
using Microsoft.AspNetCore.Mvc;
using OperationResult;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.Client.ResetPassword;

public record InitiatePasswordResetRequest(string Email);

public class InitiatePasswordResetEndpoint(ICommandDispatcher commandDispatcher) : PublicWithRequest<InitiatePasswordResetRequest>
{
    [HttpPost("clients/password/reset/initiate")]
    public override async Task<ActionResult> HandleAsync(InitiatePasswordResetRequest request)
    {
        var result = InitiateResetPasswordCommand.Create(request.Email);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }

        var dispatchResult = await commandDispatcher.DispatchAsync<InitiateResetPasswordCommand, None>(result.Data);
        return dispatchResult.IsSuccess ? Ok() : BadRequest(dispatchResult.Errors);
    }
}