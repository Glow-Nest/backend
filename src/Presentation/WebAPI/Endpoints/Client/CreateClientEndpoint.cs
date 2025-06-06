using Application.AppEntry;
using Application.AppEntry.Commands.Client;
using Microsoft.AspNetCore.Mvc;
using OperationResult;
using WebAPI.Endpoints.Common;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.Client;

public record CreateClientRequest(string FirstName, string LastName, string Email, string Password, string PhoneNumber);

public class CreateClientEndpoint(ICommandDispatcher commandDispatcher) : PublicWithRequest<CreateClientRequest>
{
    [HttpPost("clients/create")]
    public override async Task<ActionResult> HandleAsync(CreateClientRequest request)
    {
        var commandResult = CreateClientCommand.Create(request.FirstName, request.LastName,
            request.Email, request.Password, request.PhoneNumber);

        if (!commandResult.IsSuccess)
        {
            return BadRequest(commandResult.Errors);
        }

        var dispatchResult = await commandDispatcher.DispatchAsync<CreateClientCommand, None>(commandResult.Data);
        
        return dispatchResult.IsSuccess ? Ok() : BadRequest(dispatchResult.Errors);
    }
}