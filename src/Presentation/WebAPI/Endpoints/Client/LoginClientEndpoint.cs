using Microsoft.AspNetCore.Mvc;
using QueryContracts.Queries;
using QueryContracts.QueryDispatching;
using WebAPI.Endpoints.Common;

namespace WebAPI.Endpoints.Client;


public class LoginClientEndpoint: QueryEndpoint.WithRequest<LoginUserQuery>.WithResponse<LoginUserResponse>
{
    [HttpPost("clients/login")]
    public override Task<ActionResult<LoginUserResponse>> HandleAsync(LoginUserQuery request, IQueryDispatcher queryDispatcher)
    {
        var query = new LoginUserQuery(request.Email, request.Password);
        var result = queryDispatcher.DispatchAsync(query).Result;
        return Task.FromResult<ActionResult<LoginUserResponse>>(Ok(result));
    }
}