using Application.AppEntry;
using Application.AppEntry.Commands.ServiceCategory;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.Category;

public record UpdateCategoryRequest(
    string Id,
    string Name,
    string Description,
    List<string>? MediaUrls
);

public class UpdateCategoryEndpoint : ProtectedOwnerWithRequest<UpdateCategoryRequest>
{
    [HttpPost("category/update")]
    public override async Task<ActionResult> HandleAsync(UpdateCategoryRequest request, ICommandDispatcher commandDispatcher)
    {
        var commandResult = UpdateCategoryCommand.Create(request.Id, request.Name, request.Description, request.MediaUrls);
        if (!commandResult.IsSuccess)
        {
            return await Task.FromResult<ActionResult>(BadRequest(commandResult.Errors));
        }

        var dispatchResult = await commandDispatcher.DispatchAsync(commandResult.Data);
        return dispatchResult.IsSuccess ? Ok() : BadRequest(dispatchResult.Errors);
    }
}