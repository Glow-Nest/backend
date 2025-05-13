using OperationResult;
using QueryContracts.Contracts;

namespace QueryContracts.Queries.Schedule;

public class GetBlockedTime()
{
    public record BlockedTimeDto(string StartTime, string EndTime, string ScheduleDate, string Reason);
    public record Query(string ScheduleDate) : IQuery<Result<Answer>>;
    public record Answer(List<BlockedTimeDto> BlockedTimes);
}
