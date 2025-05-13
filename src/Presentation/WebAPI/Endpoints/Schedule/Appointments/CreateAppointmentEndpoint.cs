using Application.AppEntry;
using Application.AppEntry.Commands.Schedule;
using Microsoft.AspNetCore.Mvc;
using OperationResult;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.Schedule.Appointments;

public record CreateAppointmentRequest(string AppointmentNote, string AppointmentDate, string BookedByClient, List<string> ServiceIds, List<string> CategoryIds, string StartTime, string EndTime);

public class CreateAppointmentEndpoint(ICommandDispatcher commandDispatcher) : ProtectedSharedWithRequest<CreateAppointmentRequest>
{
    [HttpPost("schedule/appointment/create")]
    public override async Task<ActionResult> HandleAsync(CreateAppointmentRequest request)
    {
        var commandResult = CreateAppointmentCommand.Create(
            request.AppointmentNote,
            request.StartTime,
            request.EndTime,
            request.AppointmentDate,
            request.ServiceIds,
            request.CategoryIds,
            request.BookedByClient
        );

        if (!commandResult.IsSuccess) 
        {
            return await Task.FromResult<ActionResult>(BadRequest(commandResult.Errors));
        }

        var dispatchResult = await commandDispatcher.DispatchAsync<CreateAppointmentCommand, None>(commandResult.Data);

        return dispatchResult.IsSuccess ? Ok() : BadRequest(dispatchResult.Errors);
    }
}