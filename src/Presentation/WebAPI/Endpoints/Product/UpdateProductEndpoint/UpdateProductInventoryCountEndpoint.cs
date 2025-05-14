using Application.AppEntry;
using Application.AppEntry.Commands.Product.UpdateProduct;
using Microsoft.AspNetCore.Mvc;
using OperationResult;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.Product.UpdateProductEndpoint;


public record UpdateProductInventoryCountRequest(
    string Id,
    int InventoryCount
);
public class UpdateProductInventoryCountEndpoint(ICommandDispatcher commandDispatcher) : ProtectedOwnerWithRequest<UpdateProductInventoryCountRequest>
{
    [HttpPost("product/update/inventoryCount")]
    public override async Task<ActionResult> HandleAsync(UpdateProductInventoryCountRequest request)
    {
        var commandResult = UpdateProductInventoryCountCommand.Create(request.Id, request.InventoryCount);
        if (!commandResult.IsSuccess)
        {
            return await Task.FromResult<ActionResult>(BadRequest(commandResult.Errors));
        }

        var dispatchResult = await  commandDispatcher.DispatchAsync<UpdateProductInventoryCountCommand, None>(commandResult.Data);
        return dispatchResult.IsSuccess ? Ok() : BadRequest(dispatchResult.Errors);
    }
}