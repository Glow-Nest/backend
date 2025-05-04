using Microsoft.AspNetCore.Mvc;
using QueryContracts.Queries.Schedule;
using QueryContracts.QueryDispatching;
using WebAPI.Endpoints.Common.Query;

namespace WebAPI.Endpoints.Schedule;

public class GetAppointmentsByDateEndpoint(IQueryDispatcher queryDispatcher) : PublicQueryWithRequestAndResponse<GetAppointmentsByDateEndpoint.Request, GetAppointmentsByDateEndpoint.Response>
{
    [HttpPost("schedule/appointments")]
    public override async Task<ActionResult<Response>> HandleAsync(Request request)
    {
        var query = new GetAppointmentsByDate.Query(request.ScheduleDate, request.RequestBody.Mode);

        var dispatchResult = await queryDispatcher.DispatchAsync(query);
        if (!dispatchResult.IsSuccess)
        {
            return BadRequest(dispatchResult.Errors);
        }

        var response = new Response(dispatchResult.Data.Appointments.Select(x => new AppointmentDto(
            x.AppointmentDate,
            x.StartTime,
            x.EndTime,
            x.ClientName,
            x.Services
        )).ToList());

        return Ok(response);
    }
    
    public record AppointmentDto(string AppointmentDate, TimeOnly StartTime, TimeOnly EndTime, string ClientName, List<string> Services);
    public record Body(string Mode);
    public new record Request([FromQuery] string ScheduleDate, [FromBody] Body RequestBody);
    public new record Response(List<AppointmentDto> Appointments);
}