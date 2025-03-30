using Application.AppEntry;
using Application.AppEntry.Commands.Client;
using Application.Login;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Endpoints.Common;

namespace WebAPI.Endpoints.Client;

public record LoginClientRequest(string Email, string Password);

public class LoginClientEndpoint: CommandEndpoint.WithRequest<LoginClientRequest>.WithResponse<LoginResponse>
{
    [HttpPost("clients/login")]
    public override async Task<ActionResult<LoginResponse>> HandleAsync(LoginClientRequest request, ICommandDispatcher commandDispatcher)
    {
        var commandResult = LoginUserCommand.Create(request.Email, request.Password);
        if (!commandResult.IsSuccess)
        {
            return BadRequest(commandResult.Errors);
        }

        var dispatchResult = await commandDispatcher.DispatchAsync(commandResult.Data);
        return dispatchResult.IsSuccess ? Ok() : BadRequest(dispatchResult.Errors);
    }
}