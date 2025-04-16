using Application.AppEntry;
using Application.AppEntry.Commands.Schedule;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Endpoints.Common;

namespace WebAPI.Endpoints.Appointment;

public record CreateAppointmentRequest(string AppointmentNote, DateOnly AppointmentDate, Guid BookedByClient, List<Guid> ServiceIds, DateTime StartTime, DateTime EndTime);

public class CreateAppointmentEndpoint : CommandEndpoint.WithRequest<CreateAppointmentRequest>.WithoutResponse
{
    [HttpPost("appointments/create")]
    public override async Task<ActionResult> HandleAsync(CreateAppointmentRequest request, ICommandDispatcher commandDispatcher)
    {
        var commandResult = CreateAppointmentCommand.Create(
            request.AppointmentNote,
            request.StartTime.ToString("HH:mm"),
            request.EndTime.ToString("HH:mm"),
            request.AppointmentDate.ToString("yyyy-MM-dd"),
            request.ServiceIds.Select(x => x.ToString()).ToList(),
            request.BookedByClient.ToString()
        );

        if (!commandResult.IsSuccess) 
        {
            return await Task.FromResult<ActionResult>(BadRequest(commandResult.Errors));
        }

        var dispatchResult = await commandDispatcher.DispatchAsync(commandResult.Data);

        return dispatchResult.IsSuccess ? Ok() : BadRequest(dispatchResult.Errors);
    }
}