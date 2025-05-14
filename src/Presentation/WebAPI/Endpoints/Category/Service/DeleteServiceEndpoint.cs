using Application.AppEntry;
using Application.AppEntry.Commands.ServiceCategory;
using Microsoft.AspNetCore.Mvc;
using OperationResult;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.Category.Service;

public record DeleteServiceRequest(
    string CategoryId,
    string Id
);

public class DeleteServiceEndpoint(ICommandDispatcher commandDispatcher) : ProtectedOwnerWithRequest<DeleteServiceRequest>
{
    [HttpPost("category/service/delete")]
    public override async Task<ActionResult> HandleAsync(DeleteServiceRequest request)
    {
        var commandResult = DeleteServiceCommand.Create(request.CategoryId, request.Id);
        if (!commandResult.IsSuccess)
        {
            return await Task.FromResult<ActionResult>(BadRequest(commandResult.Errors));
        }

        var dispatchResult = await commandDispatcher.DispatchAsync<DeleteServiceCommand, None>(commandResult.Data);
        return dispatchResult.IsSuccess ? Ok() : BadRequest(dispatchResult.Errors);
    }
}