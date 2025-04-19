using Domain.Common.BaseClasses;
using Domain.Common.OperationResult;

namespace Domain.Aggregates.Schedule.Values.BlockedTime;

public class BlockReason : ValueObject
{
    public string Value { get; private set; }

    protected BlockReason(string value)
    {
        Value = value;
    }

    public static Result<BlockReason> Create(string appointmentNote)
    {
        if (string.IsNullOrWhiteSpace(appointmentNote))
        {
            return Result<BlockReason>.Fail(ScheduleErrorMessage.EmptyBlockReason());
        }
        
        var note = new BlockReason(appointmentNote);
        return Result<BlockReason>.Success(note);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}