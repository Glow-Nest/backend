using Application.AppEntry;
using Application.AppEntry.Commands.Product.UpdateProduct;
using Microsoft.AspNetCore.Mvc;
using OperationResult;
using WebAPI.Endpoints.Common.Query;

namespace WebAPI.Endpoints.Product.UpdateProductEndpoint;

public record UpdateProductNameRequest(
    string Id,
    string Name
);

public class UpdateProductNameEndpoint(ICommandDispatcher commandDispatcher) : ProtectedQueryWithRequest<UpdateProductNameRequest>
{
    [HttpPost("product/update/name")]
    public override async Task<ActionResult> HandleAsync(UpdateProductNameRequest request)
    {
        var commandResult = UpdateProductNameCommand.Create(request.Id, request.Name);
        if (!commandResult.IsSuccess)
        {
            return await Task.FromResult<ActionResult>(BadRequest(commandResult.Errors));
        }

        var dispatchResult = await  commandDispatcher.DispatchAsync<UpdateProductNameCommand, None>(commandResult.Data);
        return dispatchResult.IsSuccess ? Ok() : BadRequest(dispatchResult.Errors);
    }
}