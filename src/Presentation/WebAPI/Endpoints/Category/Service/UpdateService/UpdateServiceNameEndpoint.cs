using Application.AppEntry;
using Application.AppEntry.Commands.ServiceCategory.UpdateServiceCommand;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.Category.Service.UpdateService;


public record UpdateServiceNameRequest(
    string CategoryId,
    string Id,
    string Name
);

public class UpdateServiceNameEndpoint(ICommandDispatcher commandDispatcher) : ProtectedOwnerWithRequest<UpdateServiceNameRequest>
{
    [HttpPost("service/update/name")]
    public override async Task<ActionResult> HandleAsync(UpdateServiceNameRequest request)
    {
        var commandResult = UpdateServiceNameCommand.Create(request.CategoryId, request.Id, request.Name);
        if (!commandResult.IsSuccess)
        {
            return await Task.FromResult<ActionResult>(BadRequest(commandResult.Errors));
        }

        var dispatchResult = await commandDispatcher.DispatchAsync(commandResult.Data);
        return dispatchResult.IsSuccess ? Ok() : BadRequest(dispatchResult.Errors);
    }
}