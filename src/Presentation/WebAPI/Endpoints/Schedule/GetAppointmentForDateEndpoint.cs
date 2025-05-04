using Microsoft.AspNetCore.Mvc;
using QueryContracts.Queries.Schedule;
using QueryContracts.QueryDispatching;
using WebAPI.Endpoints.Common.Query;

namespace WebAPI.Endpoints.Schedule;

public class GetAppointmentForDateEndpoint(IQueryDispatcher queryDispatcher) : PublicQueryWithRequestAndResponse<GetAppointmentsForDate.Query, GetAppointmentsForDate.Answer>
{
    [HttpPost("schedule/appointments")]
    public override async Task<ActionResult<GetAppointmentsForDate.Answer>> HandleAsync([FromQuery] GetAppointmentsForDate.Query request)
    {
        var dispatchResult = await queryDispatcher.DispatchAsync(request);
        if (!dispatchResult.IsSuccess)
        {
            return BadRequest(dispatchResult.Errors);
        }

        return Ok(dispatchResult.Data);
    }
}