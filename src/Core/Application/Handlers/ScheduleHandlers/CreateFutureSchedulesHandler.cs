using Application.AppEntry;
using Application.AppEntry.Commands.Schedule;
using Domain.Aggregates.Schedule;
using Domain.Common.OperationResult;

namespace Application.Handlers.ScheduleHandlers;

public class CreateFutureSchedulesHandler(IScheduleRepository scheduleRepository) : ICommandHandler<CreateFutureSchedulesCommand>
{
    private readonly IScheduleRepository _scheduleRepository = scheduleRepository;
    
    public async Task<Result> HandleAsync(CreateFutureSchedulesCommand command)
    {
        var now = DateTime.Now;
        var tasks = new List<Task>();
        
        var schedulesToAdd = new List<Schedule>();

        for (int i = 0; i < command.DaysAhead; i++)
        {
            var date = DateOnly.FromDateTime(now.AddDays(i));
            var existing = await _scheduleRepository.GetScheduleByDateAsync(date);
            if (!existing.IsSuccess)
            {
                schedulesToAdd.Add(Schedule.CreateSchedule(date).Data);
            }
        }

        if (schedulesToAdd.Any())
        {
            await _scheduleRepository.AddRangeAsync(schedulesToAdd);
        }
        
        return Result.Success();
    }
}