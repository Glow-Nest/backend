using Application.AppEntry;
using Application.AppEntry.Commands.Service;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.Service;

public record CreateServiceRequest(string Name, string Description, double Price, string Duration, List<string> MediaUrls);

public class CreateServiceEndpoint :ProtectedWithRequest<CreateServiceRequest>
{
    [HttpPost("service/create")]
    public override async Task<ActionResult> HandleAsync(CreateServiceRequest request, ICommandDispatcher commandDispatcher)
    {
        var commandResult = CreateServiceCommand.Create(
            request.Name,
            request.Description,
            request.Price,
            request.Duration,
            request.MediaUrls
        );

        if (!commandResult.IsSuccess) 
        {
            return await Task.FromResult<ActionResult>(BadRequest(commandResult.Errors));
        }

        var dispatchResult = await commandDispatcher.DispatchAsync(commandResult.Data);

        return dispatchResult.IsSuccess ? Ok() : BadRequest(dispatchResult.Errors);
    }
}