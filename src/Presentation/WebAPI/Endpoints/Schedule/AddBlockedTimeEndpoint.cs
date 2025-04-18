using Application.AppEntry;
using Application.AppEntry.Commands.Schedule;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.Schedule;

public record AddBlockedTimeRequest(string StartTime, string EndTime, string ScheduleDate);

public class AddBlockedTimeEndpoint : ProtectedWithRequest<AddBlockedTimeRequest>
{
    [HttpPost("schedule/blockTime/add")]
    public override async Task<ActionResult> HandleAsync(AddBlockedTimeRequest request,
        ICommandDispatcher commandDispatcher)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(request.StartTime))
            errors.Add("StartTime must not be empty.");

        if (string.IsNullOrWhiteSpace(request.EndTime))
            errors.Add("EndTime must not be empty.");

        if (string.IsNullOrWhiteSpace(request.ScheduleDate))
            errors.Add("ScheduleDate must not be empty.");

        if (errors.Any())
            return BadRequest(errors);

        var commandResult = AddBlockedTimeCommand.Create(
            request.StartTime,
            request.EndTime,
            request.ScheduleDate
        );

        if (!commandResult.IsSuccess)
        {
            return BadRequest(commandResult.Errors);
        }

        var dispatchResult = await commandDispatcher.DispatchAsync(commandResult.Data);
        return dispatchResult.IsSuccess ? Ok() : BadRequest(dispatchResult.Errors);
    }
}