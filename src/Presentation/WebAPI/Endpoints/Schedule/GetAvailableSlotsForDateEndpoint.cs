using Microsoft.AspNetCore.Mvc;
using QueryContracts.Queries.Schedule;
using QueryContracts.QueryDispatching;
using WebAPI.Endpoints.Common.Query;

namespace WebAPI.Endpoints.Schedule;

public class GetAvailableSlotsForDateEndpoint : PublicQueryWithRequestAndResponse<GetAvailableSlotsForDate.Query,GetAvailableSlotsForDate.Answer>
{
    [HttpPost("schedule/availableSlots")]
    public override async Task<ActionResult<GetAvailableSlotsForDate.Answer>> HandleAsync([FromQuery] GetAvailableSlotsForDate.Query request, IQueryDispatcher queryDispatcher)
    {
        var dispatchResult = await queryDispatcher.DispatchAsync(request);
        if (!dispatchResult.IsSuccess)
        {
            return BadRequest(dispatchResult.Errors);
        }

        return Ok(dispatchResult.Data);
    }
}