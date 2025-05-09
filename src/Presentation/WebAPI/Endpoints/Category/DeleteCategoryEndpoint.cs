using Application.AppEntry;
using Application.AppEntry.Commands.ServiceCategory;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.Category;

public record DeleteCategoryRequest(
    string CategoryId
);

public class DeleteCategoryEndpoint(ICommandDispatcher commandDispatcher) : ProtectedOwnerWithRequest<DeleteCategoryRequest>
{
    [HttpPost("category/delete")]
    public override async Task<ActionResult> HandleAsync(DeleteCategoryRequest request)
    {
        var commandResult = DeleteCategoryCommand.Create(request.CategoryId);
        if (!commandResult.IsSuccess)
        {
            return await Task.FromResult<ActionResult>(BadRequest(commandResult.Errors));
        }

        var dispatchResult = await commandDispatcher.DispatchAsync(commandResult.Data);
        return dispatchResult.IsSuccess ? Ok() : BadRequest(dispatchResult.Errors);
    }
}