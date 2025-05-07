using Domain.Aggregates.Schedule;
using Microsoft.AspNetCore.Mvc;
using QueryContracts.Queries.Schedule;
using QueryContracts.QueryDispatching;
using WebAPI.Endpoints.Common.Query;

namespace WebAPI.Endpoints.Schedule.Appointments;

public class GetAppointmentsByDateEndpoint(IQueryDispatcher queryDispatcher) : ProtectedQueryWithRequestAndResponse<GetAppointmentsByDateEndpoint.Request, GetAppointmentsByDateEndpoint.Response>
{
    [HttpPost("schedule/appointments")]
    public override async Task<ActionResult<Response>> HandleAsync(Request request)
    {
        var query = new GetAppointmentsByDate.Query(request.ScheduleDate, request.mode);

        var dispatchResult = await queryDispatcher.DispatchAsync(query);
        if (!dispatchResult.IsSuccess)
        {
            var isNotFound = dispatchResult.Errors.Any(e =>
                string.Equals(e.ErrorId, ScheduleErrorMessage.NoAppointmentsFound().ErrorId, StringComparison.OrdinalIgnoreCase));

            if (isNotFound)
            {
                return NoContent(); // 204
            }

            
            return BadRequest(dispatchResult.Errors);
        }

        var response = new Response(dispatchResult.Data.Appointments.Select(x => new AppointmentDto(
            x.AppointmentId,
            x.AppointmentDate,
            x.StartTime,
            x.EndTime,
            x.ClientName,
            x.Services,
            x.AppointmentNote
        )).ToList());

        return Ok(response);
    }
    
    public record AppointmentDto(string AppointmentId, string AppointmentDate, TimeOnly StartTime, TimeOnly EndTime, string ClientName, List<string> Services, string AppointmentNote);
    public new record Request(string ScheduleDate, string mode);
    public new record Response(List<AppointmentDto> Appointments);
}