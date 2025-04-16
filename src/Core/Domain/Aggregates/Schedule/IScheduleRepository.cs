using Domain.Aggregates.DailyAppointmentSchedule.Values.DailySchedule;
using Domain.Common.OperationResult;
using Domain.Common.Repositories;

namespace Domain.Aggregates.Schedule;

public interface IScheduleRepository : IGenericRepository<Schedule, ScheduleId>
{
    Task<Result<Schedule>> GetScheduleByDateAsync(DateOnly date);
}