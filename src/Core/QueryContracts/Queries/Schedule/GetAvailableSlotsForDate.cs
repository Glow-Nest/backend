using Domain.Aggregates.Schedule.Values;
using Domain.Common.OperationResult;
using QueryContracts.Contracts;

namespace QueryContracts.Queries.Schedule;

public class GetAvailableSlotsForDate
{
    public record Query(string ScheduleDate) : IQuery<Result<Answer>>;
    public record Answer(Dictionary<string, List<TimeSlot>> TimeSlots);
}