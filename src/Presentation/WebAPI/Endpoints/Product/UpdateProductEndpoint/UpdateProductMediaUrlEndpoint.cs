using Application.AppEntry;
using Application.AppEntry.Commands.Product.UpdateProduct;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.Product.UpdateProductEndpoint;


public record UpdateProductMediaUrlRequest(
    string Id,
    string MediaUrl
);

public class UpdateProductMediaUrlEndpoint(ICommandDispatcher commandDispatcher): ProtectedOwnerWithRequest<UpdateProductMediaUrlRequest>
{
    [HttpPost("product/update/mediaUurl")]
    public override async Task<ActionResult> HandleAsync(UpdateProductMediaUrlRequest request)
    {
        var commandResult = UpdateProductImageUrlCommand.Create(request.Id, request.MediaUrl);
        if (!commandResult.IsSuccess)
        {
            return await Task.FromResult<ActionResult>(BadRequest(commandResult.Errors));
        }

        var dispatchResult = await commandDispatcher.DispatchAsync(commandResult.Data);
        return dispatchResult.IsSuccess ? Ok() : BadRequest(dispatchResult.Errors);
    }
}