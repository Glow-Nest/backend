using Application.AppEntry;
using Application.AppEntry.Commands.ServiceCategory;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.Service;

public record AddServiceRequest(
    string Name,
    double Price,
    string Duration,
    string CategoryId
);

public class AddServiceEndpoint(ICommandDispatcher commandDispatcher) : ProtectedOwnerWithRequest<AddServiceRequest>
{
    [HttpPost("category/service/add")]
    public override async Task<ActionResult> HandleAsync(AddServiceRequest request)
    {
        var commandResult = AddServiceInCategoryCommand.Create(request.Name, request.Price, request.Duration, request.CategoryId);
        if (!commandResult.IsSuccess)
        {
            return await Task.FromResult<ActionResult>(BadRequest(commandResult.Errors));
        }
        
        var dispatchResult = await commandDispatcher.DispatchAsync(commandResult.Data);
        return dispatchResult.IsSuccess ? Ok() : BadRequest(dispatchResult.Errors);
    }
}