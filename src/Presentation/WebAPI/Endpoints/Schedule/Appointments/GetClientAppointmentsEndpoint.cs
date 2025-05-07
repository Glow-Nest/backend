using Domain.Aggregates.Schedule;
using Microsoft.AspNetCore.Mvc;
using QueryContracts.Queries.Schedule;
using QueryContracts.QueryDispatching;
using WebAPI.Endpoints.Common.Query;

namespace WebAPI.Endpoints.Schedule.Appointments;

public class GetClientAppointmentsEndpoint(IQueryDispatcher queryDispatcher) : ProtectedSharedQueryWithRequestAndResponse<GetClientAppointmentsEndpoint.Request, GetClientAppointmentsEndpoint.Response>
{
    public new record Request(string ClientId, string TimeFrame);
    public new record Response(List<AppointmentDto> Appointments);
    public record AppointmentDto(string AppointmentId, string AppointmentDate, TimeOnly StartTime, TimeOnly EndTime, string ClientName, List<string> Services, string AppointmentNote);
    
    [HttpPost("schedule/appointments/clients")]
    public override async Task<ActionResult<Response>> HandleAsync([FromBody] Request request)
    {
        var isParse = Enum.TryParse(request.TimeFrame, ignoreCase: true, out AppointmentTimeFrame timeFrame);
        if (!isParse)
        {
            return await Task.FromResult<ActionResult<Response>>(BadRequest(ScheduleErrorMessage.InvalidAppointmentTimeFrame()));
        }
        

        var query = new GetClientsAppointment.Query(request.ClientId, timeFrame);
        var dispatchResult = await queryDispatcher.DispatchAsync(query);
        
        if (!dispatchResult.IsSuccess)
        {
            var isNotFound = dispatchResult.Errors.Any(e =>
                string.Equals(e.ErrorId, ScheduleErrorMessage.NoAppointmentsFound().ErrorId, StringComparison.OrdinalIgnoreCase));

            if (isNotFound)
            {
                return NoContent();
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
}