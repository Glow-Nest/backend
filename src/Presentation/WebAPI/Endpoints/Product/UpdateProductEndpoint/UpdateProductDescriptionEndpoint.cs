using Application.AppEntry;
using Application.AppEntry.Commands.Product.UpdateProduct;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.Product.UpdateProductEndpoint;

public record UpdateProductDescriptionRequest(
    string Id,
    string Description
);

public class UpdateProductDescriptionEndpoint(ICommandDispatcher commandDispatcher) : ProtectedOwnerWithRequest<UpdateProductDescriptionRequest>
{
    [HttpPost("product/update/description")]
    public async override Task<ActionResult> HandleAsync(UpdateProductDescriptionRequest request)
    {
        var commandResult = UpdateProductDescriptionCommand.Create(request.Id, request.Description);
        if (!commandResult.IsSuccess)
        {
            return await Task.FromResult<ActionResult>(BadRequest(commandResult.Errors));
        }

        var dispatchResult = await commandDispatcher.DispatchAsync(commandResult.Data);
        return dispatchResult.IsSuccess ? Ok() : BadRequest(dispatchResult.Errors);
    }
}