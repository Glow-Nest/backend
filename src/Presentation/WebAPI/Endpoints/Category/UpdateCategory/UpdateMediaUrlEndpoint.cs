using Application.AppEntry;
using Application.AppEntry.Commands.ServiceCategory;
using Application.AppEntry.Commands.ServiceCategory.UpdateCategoryCommand;
using Microsoft.AspNetCore.Mvc;
using OperationResult;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.Category.UpdateCategory;

public record UpdateMediaUrlRequest(
    string Id,
    List<string> MediaUrls
);

public class UpdateMediaUrlEndpoint(ICommandDispatcher commandDispatcher) : ProtectedOwnerWithRequest<UpdateMediaUrlRequest>
{
    [HttpPost("category/update/media-url")]
    public override async Task<ActionResult> HandleAsync(UpdateMediaUrlRequest request)
    {
        var commandResult = UpdateMediaUrlCommand.Create(request.Id, request.MediaUrls);
        if (!commandResult.IsSuccess)
        {
            return await Task.FromResult<ActionResult>(BadRequest(commandResult.Errors));
        }

        var dispatchResult = await commandDispatcher.DispatchAsync<UpdateMediaUrlCommand, None>(commandResult.Data);
        return dispatchResult.IsSuccess ? Ok() : BadRequest(dispatchResult.Errors);
    }
}