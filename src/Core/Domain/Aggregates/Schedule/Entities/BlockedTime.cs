using Domain.Aggregates.Schedule.Values;
using Domain.Aggregates.Schedule.Values.BlockedTimeValues;
using Domain.Common.BaseClasses;
using Domain.Common.Contracts;
using Domain.Common.OperationResult;

namespace Domain.Aggregates.Schedule.Entities;

public class BlockedTime : Entity<BlockedTimeId>
{
    internal DateOnly ScheduledDate { get; }
    internal TimeSlot TimeSlot { get; }
    internal BlockedTimeId BlockedTimeId { get; }
    internal BlockReason Reason { get; }

    public BlockedTime(BlockedTimeId id) : base(id)
    {
    }
    
    protected BlockedTime(BlockedTimeId id, DateOnly scheduledDate, TimeSlot timeSlot, BlockReason reason) : base(id)
    {
        ScheduledDate = scheduledDate;
        TimeSlot = timeSlot;
        BlockedTimeId = id;
        Reason = reason;
    }

    public static Result<BlockedTime> Create(DateOnly scheduleDate, TimeSlot timeSlot, BlockReason reason, IDateTimeProvider dateTimeProvider)
    {
        var now = dateTimeProvider.GetNow();
        var dateOnly = DateOnly.FromDateTime(now);
        var timeOnly = TimeOnly.FromDateTime(now);

        // check if the date is in the past
        if (scheduleDate < dateOnly ||
            (scheduleDate == dateOnly && timeSlot.Start < timeOnly))
        {
            return Result<BlockedTime>.Fail(ScheduleErrorMessage.BlockedTimeInPast());
        }

        var id = BlockedTimeId.Create();
        
        var blockedTime = new BlockedTime(id, scheduleDate, timeSlot, reason);
        return Result<BlockedTime>.Success(blockedTime);
    }
}