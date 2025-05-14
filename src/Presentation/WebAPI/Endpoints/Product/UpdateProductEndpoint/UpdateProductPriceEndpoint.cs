using Application.AppEntry;
using Application.AppEntry.Commands.Product.UpdateProduct;
using Microsoft.AspNetCore.Mvc;
using OperationResult;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.Product.UpdateProductEndpoint;

public record UpdateProductPrice(string ProductId, int Price);

public class UpdateProductPriceEndpoint(ICommandDispatcher commandDispatcher) : ProtectedOwnerWithRequest<UpdateProductPrice>
{
    [HttpPost("product/update/price")]
    public override async Task<ActionResult> HandleAsync(UpdateProductPrice request)
    {
        var commandResult = UpdateProductPriceCommand.Create(request.ProductId, request.Price);
        if (!commandResult.IsSuccess)
        {
            return await Task.FromResult<ActionResult>(BadRequest(commandResult.Errors));
        }

        var dispatchResult = await  commandDispatcher.DispatchAsync<UpdateProductPriceCommand, None>(commandResult.Data);
        return dispatchResult.IsSuccess ? Ok() : BadRequest(dispatchResult.Errors);
    }
}