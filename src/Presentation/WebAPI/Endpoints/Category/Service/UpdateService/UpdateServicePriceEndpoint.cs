using Application.AppEntry;
using Application.AppEntry.Commands.ServiceCategory.UpdateServiceCommand;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.Category.Service.UpdateService;

public record UpdateServicePriceRequest(
    string CategoryId,
    string Id,
    double Price
);

public class UpdateServicePriceEndpoint(ICommandDispatcher commandDispatcher) : ProtectedOwnerWithRequest<UpdateServicePriceRequest>
{
    [HttpPost("service/update/price")]
    public override async Task<ActionResult> HandleAsync(UpdateServicePriceRequest request)
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