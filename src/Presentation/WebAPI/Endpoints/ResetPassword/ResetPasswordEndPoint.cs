using Application.AppEntry;
using Application.AppEntry.Commands.Client;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Endpoints.Common;

namespace WebAPI.Endpoints.Client;

public record CompletePasswordResetRequest(string Email, string NewPassword, string ConfirmPassword);

public class ResetPasswordEndPoint : CommandEndpoint.WithRequest<CompletePasswordResetRequest>.WithoutResponse
{
    [HttpPost("clients/password/reset")]
    public override async Task<ActionResult> HandleAsync(CompletePasswordResetRequest request, ICommandDispatcher commandDispatcher)
    {
        var result = ResetPasswordCommand.Create(request.Email, request.NewPassword, request.ConfirmPassword);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }

        var dispatchResult = await commandDispatcher.DispatchAsync(result.Data);
        return dispatchResult.IsSuccess ? Ok() : BadRequest(dispatchResult.Errors);
    }
}