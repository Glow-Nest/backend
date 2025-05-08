using Application.AppEntry;
using Application.AppEntry.Commands.ServiceCategory;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.Service.UpdateService;

public record UpdateServicePriceRequest(
    string CategoryId,
    string Id,
    double Price
);

public class UpdateServicePriceEndpoint : ProtectedOwnerWithRequest<UpdateServicePriceRequest>
{
    [HttpPost("service/update/price")]
    public override async Task<ActionResult> HandleAsync(UpdateServicePriceRequest request, ICommandDispatcher commandDispatcher)
    {
        var commandResult = UpdateServicePriceCommand.Create(request.CategoryId, request.Id, request.Price);
        if (!commandResult.IsSuccess)
        {
            return await Task.FromResult<ActionResult>(BadRequest(commandResult.Errors));
        }

        var dispatchResult = await commandDispatcher.DispatchAsync(commandResult.Data);
        return dispatchResult.IsSuccess ? Ok() : BadRequest(dispatchResult.Errors);
    }
}