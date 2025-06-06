using Domain.Aggregates.Schedule.Values;
using Domain.Common.Repositories;
using OperationResult;

namespace Domain.Aggregates.Schedule;

public interface IScheduleRepository : IGenericRepository<Schedule, ScheduleId>
{
    Task<Result<Schedule>> GetScheduleByDateAsync(DateOnly date);
}