using Application.AppEntry;
using Application.AppEntry.Commands.Schedule;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Endpoints.Common;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.Schedule;

public record CreateAppointmentRequest(string AppointmentNote, string AppointmentDate, string BookedByClient, List<string> ServiceIds, string StartTime, string EndTime);

public class CreateAppointmentEndpoint : ProtectedSharedWithRequest<CreateAppointmentRequest>
{
    [HttpPost("schedule/appointment/create")]
    public override async Task<ActionResult> HandleAsync(CreateAppointmentRequest request, ICommandDispatcher commandDispatcher)
    {
        var commandResult = CreateAppointmentCommand.Create(
            request.AppointmentNote,
            request.StartTime,
            request.EndTime,
            request.AppointmentDate,
            request.ServiceIds,
            request.BookedByClient
        );

        if (!commandResult.IsSuccess) 
        {
            return await Task.FromResult<ActionResult>(BadRequest(commandResult.Errors));
        }

        var dispatchResult = await commandDispatcher.DispatchAsync(commandResult.Data);

        return dispatchResult.IsSuccess ? Ok() : BadRequest(dispatchResult.Errors);
    }
}