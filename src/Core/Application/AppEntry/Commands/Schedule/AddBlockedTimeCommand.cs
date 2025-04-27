using Domain.Aggregates.Schedule.Values;
using Domain.Aggregates.Schedule.Values.BlockedTimeValues;
using Domain.Common;
using Domain.Common.OperationResult;

namespace Application.AppEntry.Commands.Schedule;

public class AddBlockedTimeCommand(TimeSlot timeSlot, DateOnly scheduleDate, BlockReason reason)
{
    internal TimeSlot timeSlot = timeSlot;
    internal DateOnly scheduleDate = scheduleDate;
    internal BlockReason reason = reason;

    public static Result<AddBlockedTimeCommand> Create(string startTimeStr, string endTimeStr, string dateStr, string reasonStr)
    {
        var dateParse = DateOnly.TryParse(dateStr, out var date);
        if (!dateParse)
        {
            return Result<AddBlockedTimeCommand>.Fail(GenericErrorMessage.ErrorParsingDate());
        }

        var startTimeParse = TimeOnly.TryParse(startTimeStr, out var startTime);
        var endTimeParse = TimeOnly.TryParse(endTimeStr, out var endTime);
        
        if (!startTimeParse || !endTimeParse)
        {
            return Result<AddBlockedTimeCommand>.Fail(GenericErrorMessage.ErrorParsingTime());
        }

        var timeSlotResult = TimeSlot.Create(startTime, endTime);
        if (!timeSlotResult.IsSuccess)
        {
            return Result<AddBlockedTimeCommand>.Fail(timeSlotResult.Errors);
        }
        
        var reasonResult = BlockReason.Create(reasonStr);
        if (!reasonResult.IsSuccess)
        {
            return Result<AddBlockedTimeCommand>.Fail(reasonResult.Errors);
        }

        var blockedTimeCommand = new AddBlockedTimeCommand(timeSlotResult.Data, date, reasonResult.Data);
        return Result<AddBlockedTimeCommand>.Success(blockedTimeCommand);
    }
}