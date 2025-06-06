using Application.AppEntry;
using Application.AppEntry.Commands.Schedule;
using Domain.Aggregates.Schedule;
using Domain.Common.Contracts;
using OperationResult;

namespace Application.Handlers.ScheduleHandlers;

public class AddBlockedTimeHandler(IScheduleRepository scheduleRepository, IDateTimeProvider dateTimeProvider) : ICommandHandler<AddBlockedTimeCommand, None>
{
    private readonly IScheduleRepository _scheduleRepository = scheduleRepository;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

    public async Task<Result<None>> HandleAsync(AddBlockedTimeCommand command)
    {
        var scheduleResult = await GetOrCreateScheduleAsync(command.scheduleDate);
        if (!scheduleResult.IsSuccess)
        {
            return Result<None>.Fail(scheduleResult.Errors);
        }
        
        var schedule = scheduleResult.Data;
        var result = await schedule.AddBlockedTime(command.timeSlot, command.reason, _dateTimeProvider);
        if (!result.IsSuccess)
        {
            return Result<None>.Fail(result.Errors);
        }

        return Result<None>.Success(None.Value);
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