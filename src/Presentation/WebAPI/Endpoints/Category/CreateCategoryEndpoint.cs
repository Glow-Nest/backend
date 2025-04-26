using Application.AppEntry;
using Application.AppEntry.Commands.ServiceCategory;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.Category;

public record CreateCategoryRequest(
    string Name,
    string Description,
    List<string> MediaUrls
);

public class CreateCategoryEndpoint : ProtectedOwnerWithRequest<CreateCategoryRequest>
{
    [HttpPost("category/create")]
    public override async Task<ActionResult> HandleAsync(CreateCategoryRequest request, ICommandDispatcher commandDispatcher)
    {
        var commandResult = CreateCategoryCommand.Create(request.Name, request.Description, request.MediaUrls);
        if (!commandResult.IsSuccess)
        {
            return await Task.FromResult<ActionResult>(BadRequest(commandResult.Errors));
        }
        
        var dispatchResult = await commandDispatcher.DispatchAsync(commandResult.Data);
        return dispatchResult.IsSuccess ? Ok() : BadRequest(dispatchResult.Errors);
    }
}