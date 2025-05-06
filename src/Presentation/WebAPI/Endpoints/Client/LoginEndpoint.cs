using Microsoft.AspNetCore.Mvc;
using QueryContracts.Queries;
using QueryContracts.QueryDispatching;
using WebAPI.Endpoints.Common.Query;

namespace WebAPI.Endpoints.Client;

public class LoginEndpoint(IQueryDispatcher queryDispatcher): PublicQueryWithRequestAndResponse<LoginUserQuery, LoginUserResponse>
{
    [HttpPost("clients/login")]
    public override Task<ActionResult<LoginUserResponse>> HandleAsync(LoginUserQuery request)
    {
        var query = new LoginUserQuery(request.Email, request.Password);
        var dispatchResult = queryDispatcher.DispatchAsync(query).Result;
        if (dispatchResult.IsSuccess)
        {
            return Task.FromResult<ActionResult<LoginUserResponse>>(Ok(dispatchResult.Data));
        }
        return Task.FromResult<ActionResult<LoginUserResponse>>(BadRequest(dispatchResult.Errors));
    }
}