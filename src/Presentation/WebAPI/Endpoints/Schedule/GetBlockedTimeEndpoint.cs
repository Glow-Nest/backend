using Microsoft.AspNetCore.Mvc;
using QueryContracts.Queries.Schedule;
using QueryContracts.QueryDispatching;
using WebAPI.Endpoints.Common.Query;

namespace WebAPI.Endpoints.Schedule;


public class GetBlockedTimeEndpoint : ProtectedQueryWithRequestAndResponse<GetBlockedTimeQuery, GetBlockedTimeResponse>
{
    [HttpPost("schedule/blockTime")]
    public override async Task<ActionResult<GetBlockedTimeResponse>> HandleAsync([FromQuery] GetBlockedTimeQuery request, IQueryDispatcher queryDispatcher)
    {
        var dispatchResult = await queryDispatcher.DispatchAsync(request);
        if (dispatchResult.IsSuccess)
        {
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(dispatchResult.Data));
            return await Task.FromResult<ActionResult<GetBlockedTimeResponse>>(Ok(dispatchResult.Data));
        }
        return await Task.FromResult<ActionResult<GetBlockedTimeResponse>>(BadRequest(dispatchResult.Errors));
    }
}