using Microsoft.AspNetCore.Mvc;
using QueryContracts.Queries.Schedule;
using QueryContracts.QueryDispatching;
using WebAPI.Endpoints.Common.Query;

namespace WebAPI.Endpoints.Schedule;


public class GetBlockedTimeEndpoint : ProtectedQueryWithRequestAndResponse<GetBlockedTime.Query, GetBlockedTime.Answer>
{
    [HttpPost("schedule/blockTime")]
    public override async Task<ActionResult<GetBlockedTime.Answer>> HandleAsync(GetBlockedTime.Query request, IQueryDispatcher queryDispatcher)
    {
        var dispatchResult = await queryDispatcher.DispatchAsync(request);
        if (dispatchResult.IsSuccess)
        {
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(dispatchResult.Data));
            return await Task.FromResult<ActionResult<GetBlockedTime.Answer>>(Ok(dispatchResult.Data));
        }
        return await Task.FromResult<ActionResult<GetBlockedTime.Answer>>(BadRequest(dispatchResult.Errors));
    }
    
}