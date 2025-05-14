using Application.AppEntry;
using Application.AppEntry.Commands.Schedule;
using Microsoft.AspNetCore.Mvc;
using OperationResult;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.Schedule.BlockedTime;

public record AddBlockedTimeRequest(string StartTime, string EndTime, string ScheduleDate, string BlockReason);

public class AddBlockedTimeEndpoint(ICommandDispatcher commandDispatcher) : ProtectedOwnerWithRequest<AddBlockedTimeRequest>
{
    [HttpPost("schedule/blockTime/add")]
    public override async Task<ActionResult> HandleAsync(AddBlockedTimeRequest request)
    {
        var commandResult = AddBlockedTimeCommand.Create(
            request.StartTime,
            request.EndTime,
            request.ScheduleDate,
            request.BlockReason
        );

        if (!commandResult.IsSuccess)
        {
            return BadRequest(commandResult.Errors);
        }

        var dispatchResult = await commandDispatcher.DispatchAsync<AddBlockedTimeCommand, None>(commandResult.Data);
        return dispatchResult.IsSuccess ? Ok() : BadRequest(dispatchResult.Errors);
    }
}