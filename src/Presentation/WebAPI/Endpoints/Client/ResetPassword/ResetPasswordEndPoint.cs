using Application.AppEntry;
using Application.AppEntry.Commands.Client;
using Microsoft.AspNetCore.Mvc;
using OperationResult;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.Client.ResetPassword;

public record CompletePasswordResetRequest(string Email, string NewPassword, string ConfirmPassword);

public class ResetPasswordEndPoint(ICommandDispatcher commandDispatcher) : PublicWithRequest<CompletePasswordResetRequest>
{
    [HttpPost("clients/password/reset")]
    public override async Task<ActionResult> HandleAsync(CompletePasswordResetRequest request)
    {
        var result = ResetPasswordCommand.Create(request.Email, request.NewPassword, request.ConfirmPassword);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }

        var dispatchResult = await commandDispatcher.DispatchAsync<ResetPasswordCommand, None>(result.Data);
        return dispatchResult.IsSuccess ? Ok() : BadRequest(dispatchResult.Errors);
    }
}