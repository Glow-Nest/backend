using Application.AppEntry;
using Application.AppEntry.Commands.ServiceCategory.UpdateServiceCommand;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.Category.Service.UpdateService;

public record UpdateServiceDurationRequest(
    string CategoryId,
    string Id,
    string Duration
);

public class UpdateServiceDurationEndpoint(ICommandDispatcher commandDispatcher) : ProtectedOwnerWithRequest<UpdateServiceDurationRequest>
{
    [HttpPost("service/update/duration")]
    public override async Task<ActionResult> HandleAsync(UpdateServiceDurationRequest request)
    {
        var commandResult = UpdateServiceDurationCommand.Create(request.CategoryId, request.Id, TimeSpan.Parse(request.Duration));
        if (!commandResult.IsSuccess)
        {
            return await Task.FromResult<ActionResult>(BadRequest(commandResult.Errors));
        }

        var dispatchResult = await commandDispatcher.DispatchAsync(commandResult.Data);
        return dispatchResult.IsSuccess ? Ok() : BadRequest(dispatchResult.Errors);
    }
}