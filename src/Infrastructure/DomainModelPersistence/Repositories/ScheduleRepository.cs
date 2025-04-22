using Domain.Aggregates.Schedule;
using Domain.Aggregates.Schedule.Values;
using Domain.Common.OperationResult;
using DomainModelPersistence.EfcConfigs;
using DomainModelPersistence.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace DomainModelPersistence.Repositories;

public class ScheduleRepository(DomainModelContext context) : RepositoryBase<Schedule, ScheduleId>(context), IScheduleRepository
{
    public async Task<Result<Schedule>> GetScheduleByDateAsync(DateOnly date)
    {
        var schedule = await context.Set<Schedule>()
            .Include(s => s.Appointments)
            .Include(s => s.BlockedTimeSlots)
            .FirstOrDefaultAsync(s => s.ScheduleDate == date);

        return schedule is null
            ? Result<Schedule>.Fail(ScheduleErrorMessage.ScheduleNotFound(date))
            : Result<Schedule>.Success(schedule);
    }
}