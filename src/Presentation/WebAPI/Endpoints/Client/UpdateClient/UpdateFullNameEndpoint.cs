using Application.AppEntry;
using Application.AppEntry.Commands.Client.UpdateClient;
using Microsoft.AspNetCore.Mvc;
using OperationResult;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.Client.UpdateClient;


public record UpdateFullNameRequest(string Id, string FirstName, string LastName);

public class UpdateFullNameEndpoint(ICommandDispatcher commandDispatcher) : PublicWithRequest<UpdateFullNameRequest>
{
    [HttpPost("clients/update/fullname")]
    public override async Task<ActionResult> HandleAsync(UpdateFullNameRequest request)
    {
        var commandResult = UpdateFullNameCommand.Create(request.Id, request.FirstName, request.LastName);
        if (!commandResult.IsSuccess)
        {
            return await Task.FromResult<ActionResult>(BadRequest(commandResult.Errors));
        }

        var dispatchResult = await commandDispatcher.DispatchAsync<UpdateFullNameCommand, None>(commandResult.Data);
        return dispatchResult.IsSuccess ? Ok() : BadRequest(dispatchResult.Errors);
    }
}