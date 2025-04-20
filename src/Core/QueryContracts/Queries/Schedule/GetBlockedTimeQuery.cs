using Domain.Common.OperationResult;
using QueryContracts.Contracts;

namespace QueryContracts.Queries.Schedule;

public record BlockedTimeDto(string StartTime, string EndTime, string ScheduleDate, string Reason);

public record GetBlockedTimeResponse(List<BlockedTimeDto> BlockedTimes);

public record GetBlockedTimeQuery(string ScheduleDate) : IQuery<Result<GetBlockedTimeResponse>>;
