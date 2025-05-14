using Application.AppEntry;
using Application.AppEntry.Commands.Client.UpdateClient;
using Microsoft.AspNetCore.Mvc;
using OperationResult;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.Client.UpdateClient;


public record UpdatePhoneNumberRequest(string Id, string PhoneNumber);

public class UpdatePhoneNumberEndpoint(ICommandDispatcher commandDispatcher) : PublicWithRequest<UpdatePhoneNumberRequest>
{
    [HttpPost("clients/update/phoneNumber")]
    public override async Task<ActionResult> HandleAsync(UpdatePhoneNumberRequest request)
    {
        var commandResult = UpdatePhoneNumberCommand.Create(request.Id, request.PhoneNumber);
        if (!commandResult.IsSuccess)
        {
            return await Task.FromResult<ActionResult>(BadRequest(commandResult.Errors));
        }

        var dispatchResult = await commandDispatcher.DispatchAsync<UpdatePhoneNumberCommand, None>(commandResult.Data);
        return dispatchResult.IsSuccess ? Ok() : BadRequest(dispatchResult.Errors);
    }
}