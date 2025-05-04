using Domain.Aggregates.Schedule.Values;
using Microsoft.AspNetCore.Mvc;
using QueryContracts.Queries.Schedule;
using QueryContracts.QueryDispatching;
using WebAPI.Endpoints.Common.Query;

namespace WebAPI.Endpoints.Schedule;

public record GetAvailableSlotsForDateRequest([FromQuery] string SchedueleDate);
public record GetAvailableSlotsForDateResponse(Dictionary<string, List<TimeSlot>> TimeSlots);

public class GetAvailableSlotsForDateEndpoint(IQueryDispatcher queryDispatcher) : PublicQueryWithRequestAndResponse<GetAvailableSlotsForDateRequest,GetAvailableSlotsForDateResponse>
{
    [HttpPost("schedule/availableSlots")]
    public override async Task<ActionResult<GetAvailableSlotsForDateResponse>> HandleAsync(GetAvailableSlotsForDateRequest request)
    {
        var query = new GetAvailableSlotsForDate.Query(request.SchedueleDate);
        var dispatchResult = await queryDispatcher.DispatchAsync(query);
        if (!dispatchResult.IsSuccess)
        {
            return BadRequest(dispatchResult.Errors);
        }

        return Ok(dispatchResult.Data);
    }
}