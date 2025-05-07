using Application.AppEntry;
using Application.AppEntry.Commands.ServiceCategory;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.Service;

public record UpdateServiceRequest(
    string CategoryId,
    string ServiceId,
    string Name,
    string duration,
    double price
);

public class UpdateServiceEndpoint : ProtectedOwnerWithRequest<UpdateServiceRequest>
{
    [HttpPost("service/update")]
    public override async Task<ActionResult> HandleAsync(UpdateServiceRequest request, ICommandDispatcher commandDispatcher)
    {
        var commandResult = UpdateServiceCommand.Create(request.CategoryId, request.ServiceId, request.Name, request.price, TimeSpan.Parse(request.duration));
        if (!commandResult.IsSuccess)
        {
            return await Task.FromResult<ActionResult>(BadRequest(commandResult.Errors));
        }

        var dispatchResult = await commandDispatcher.DispatchAsync(commandResult.Data);
        return dispatchResult.IsSuccess ? Ok() : BadRequest(dispatchResult.Errors);
    }
}