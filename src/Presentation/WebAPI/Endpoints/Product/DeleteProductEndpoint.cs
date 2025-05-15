using Application.AppEntry;
using Application.AppEntry.Commands.Product;
using Microsoft.AspNetCore.Mvc;
using OperationResult;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.Product;

public record DeleteProductRequest(
    string Id
);

public class DeleteProductEndpoint(ICommandDispatcher commandDispatcher) : ProtectedOwnerWithRequest<DeleteProductRequest>
{
    [HttpPost("product/delete")]
    public override async Task<ActionResult> HandleAsync(DeleteProductRequest request)
    {
        var commandResult = DeleteProductCommand.Create(request.Id);
        if (!commandResult.IsSuccess)
        {
            return await Task.FromResult<ActionResult>(BadRequest(commandResult.Errors));
        }

        var dispatchResult = await  commandDispatcher.DispatchAsync<DeleteProductCommand, None>(commandResult.Data);
        return dispatchResult.IsSuccess ? Ok() : BadRequest(dispatchResult.Errors);
    }
}