using Microsoft.AspNetCore.Mvc;
using QueryContracts.Queries.Schedule;
using QueryContracts.QueryDispatching;
using WebAPI.Endpoints.Common.Query;

namespace WebAPI.Endpoints.Schedule.BlockedTime;

public class GetBlockedTimeEndpoint(IQueryDispatcher queryDispatcher) : ProtectedQueryWithRequestAndResponse<GetBlockedTimeEndpoint.Request, GetBlockedTimeEndpoint.Response>
{
    [HttpPost("schedule/blockTime")]
    public override async Task<ActionResult<Response>> HandleAsync(Request request)
    {
        var query = new GetBlockedTime.Query(request.ScheduleDate);
        var dispatchResult = await queryDispatcher.DispatchAsync(query);
        
        if (!dispatchResult.IsSuccess)
        {
            return BadRequest(dispatchResult.Errors);
        }
        
        var response = new Response(dispatchResult.Data.BlockedTimes.Select(x => new BlockedTimeDto(
            x.StartTime,
            x.EndTime,
            x.ScheduleDate,
            x.Reason
        )).ToList());
        
        return Ok(response);
    }

    public new record Request(string ScheduleDate);

    public new record Response(List<BlockedTimeDto> BlockedTimes);
    public record BlockedTimeDto(string StartTime, string EndTime, string ScheduleDate, string Reason);

}