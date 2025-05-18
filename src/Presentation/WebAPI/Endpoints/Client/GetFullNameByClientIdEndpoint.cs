using Microsoft.AspNetCore.Mvc;
using QueryContracts.Queries.User;
using QueryContracts.QueryDispatching;
using WebAPI.Endpoints.Common.Query;

namespace WebAPI.Endpoints.Client;

public class GetFullNameByClientIdEndpoint(IQueryDispatcher queryDispatcher) : PublicQueryWithRequestAndResponse<GetFullNameByClientIdEndpoint.Request, GetFullNameByClientIdEndpoint.Response>
{
    public new record Request(string ClientId);
    public new record Response(string ClientId,string FirstName, string LastName);
    
    [HttpPost("clients/{ClientId}")]
    public override async Task<ActionResult<Response>> HandleAsync([FromRoute] Request request)
    {
        var queryResult = new GetFullNameByUserId.Query(request.ClientId);

        var dispatchResult = await queryDispatcher.DispatchAsync(queryResult);
        if (!dispatchResult.IsSuccess)
        {
            return BadRequest(dispatchResult.Errors);
        }

        var client = dispatchResult.Data;
        var response = new Response(client.ClientId, client.FirstName, client.LastName);
        
        return Ok(response);
    }
}