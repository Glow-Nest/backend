using Application.AppEntry;
using Application.AppEntry.Commands.ServiceCategory;
using Application.AppEntry.Commands.ServiceCategory.UpdateCategoryCommand;
using Microsoft.AspNetCore.Mvc;
using OperationResult;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.Category.UpdateCategory;

public record UpdateCategoryDescriptionRequest(
    string Id,
    string Description
);
    
public class UpdateCategoryDescriptionEndpoint(ICommandDispatcher commandDispatcher) : ProtectedOwnerWithRequest<UpdateCategoryDescriptionRequest>
{
    [HttpPost("category/update/description")]
    public override async Task<ActionResult> HandleAsync(UpdateCategoryDescriptionRequest request)
    {
        var commandResult = UpdateCategoryDescriptionCommand.Create(request.Id, request.Description);
        if (!commandResult.IsSuccess)
        {
            return await Task.FromResult<ActionResult>(BadRequest(commandResult.Errors));
        }

        var dispatchResult = await commandDispatcher.DispatchAsync<UpdateCategoryDescriptionCommand, None>(commandResult.Data);
        return dispatchResult.IsSuccess ? Ok() : BadRequest(dispatchResult.Errors);
    }
}