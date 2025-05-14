using Application.AppEntry;
using Application.AppEntry.Commands.ServiceCategory;
using Application.AppEntry.Commands.ServiceCategory.UpdateCategoryCommand;
using Microsoft.AspNetCore.Mvc;
using OperationResult;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.Category.UpdateCategory;

public record UpdateCategoryNameRequest(
    string Id,
    string Name
);

public class UpdateCategoryNameEndpoint(ICommandDispatcher commandDispatcher) : ProtectedOwnerWithRequest<UpdateCategoryNameRequest>
{
    [HttpPost("category/update/name")]
    public override async Task<ActionResult> HandleAsync(UpdateCategoryNameRequest request)
    {
        var commandResult = UpdateCategoryNameCommand.Create(request.Id, request.Name);
        if (!commandResult.IsSuccess)
        {
            return await Task.FromResult<ActionResult>(BadRequest(commandResult.Errors));
        }

        var dispatchResult = await commandDispatcher.DispatchAsync<UpdateCategoryNameCommand, None>(commandResult.Data);
        return dispatchResult.IsSuccess ? Ok() : BadRequest(dispatchResult.Errors);
    }
}