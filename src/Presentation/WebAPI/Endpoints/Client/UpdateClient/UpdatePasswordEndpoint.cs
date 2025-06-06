﻿using Application.AppEntry;
using Application.AppEntry.Commands.Client.UpdateClient;
using Microsoft.AspNetCore.Mvc;
using OperationResult;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.Client.UpdateClient;

public record UpdatePasswordRequest(string Id, string NewPassword, string ConfirmPassword);

public class UpdatePasswordEndpoint(ICommandDispatcher commandDispatcher): PublicWithRequest<UpdatePasswordRequest>
{
    [HttpPost("clients/update/password")]
    public override async Task<ActionResult> HandleAsync(UpdatePasswordRequest request)
    {
        var commandResult = UpdatePasswordCommand.Create(request.Id, request.NewPassword, request.ConfirmPassword);
        if (!commandResult.IsSuccess)
        {
            return await Task.FromResult<ActionResult>(BadRequest(commandResult.Errors));
        }

        var dispatchResult = await commandDispatcher.DispatchAsync<UpdatePasswordCommand, None>(commandResult.Data);
        return dispatchResult.IsSuccess ? Ok() : BadRequest(dispatchResult.Errors);
    }
}