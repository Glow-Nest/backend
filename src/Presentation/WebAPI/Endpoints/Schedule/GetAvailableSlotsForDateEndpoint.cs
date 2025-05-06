using Domain.Aggregates.Schedule;
using Domain.Aggregates.Schedule.Values;
using Microsoft.AspNetCore.Mvc;
using QueryContracts.Queries.Schedule;
using QueryContracts.QueryDispatching;
using WebAPI.Endpoints.Common.Query;

namespace WebAPI.Endpoints.Schedule;

public class GetAvailableSlotsForDateEndpoint(IQueryDispatcher queryDispatcher) : PublicQueryWithRequestAndResponse<GetAvailableSlotsForDateEndpoint.Request,GetAvailableSlotsForDateEndpoint.Response>
{
    [HttpPost("schedule/availableSlots")]
    public override async Task<ActionResult<Response>> HandleAsync([FromQuery] Request request)
    {
        var query = new GetAvailableSlotsForDate.Query(request.ScheduleDate);
        var dispatchResult = await queryDispatcher.DispatchAsync(query);
        if (!dispatchResult.IsSuccess)
        {
            return BadRequest(dispatchResult.Errors);
        }


        return Ok(dispatchResult.Data);
    }
    
    public new record Request([FromQuery] string ScheduleDate);
    public new record Response(Dictionary<string, List<TimeSlot>> TimeSlots);
}