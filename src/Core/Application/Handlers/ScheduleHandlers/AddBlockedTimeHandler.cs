using Application.AppEntry;
using Application.AppEntry.Commands.Schedule;
using Domain.Aggregates.Schedule;
using Domain.Common.OperationResult;

namespace Application.Handlers.ScheduleHandlers;

public class AddBlockedTimeHandler(IScheduleRepository scheduleRepository) : ICommandHandler<AddBlockedTimeCommand>
{
    private readonly IScheduleRepository _scheduleRepository = scheduleRepository;

    public async Task<Result> HandleAsync(AddBlockedTimeCommand command)
    {
        var scheduleResult = await GetOrCreateScheduleAsync(command.scheduleDate);
        if (!scheduleResult.IsSuccess)
        {
            return Result.Fail(scheduleResult.Errors);
        }
        
        var schedule = scheduleResult.Data;
        var result = await schedule.AddBlockedTime(command.timeSlot);
        if (!result.IsSuccess)
        {
            return Result.Fail(result.Errors);
        }

        return Result.Success();
    }
    
    private async Task<Result<Schedule>> GetOrCreateScheduleAsync(DateOnly date)
    {
        var result = await _scheduleRepository.GetScheduleByDateAsync(date);
        if (result.IsSuccess)
        {
            return result;
        }

        if (!result.HasError(ScheduleErrorMessage.ScheduleNotFound(date).ErrorId))
        {
            return result;
        }

        var newScheduleResult = Schedule.CreateSchedule(date);
        if (!newScheduleResult.IsSuccess)
        {
            return newScheduleResult;
        }

        await _scheduleRepository.AddAsync(newScheduleResult.Data);
        return newScheduleResult;
    }
}