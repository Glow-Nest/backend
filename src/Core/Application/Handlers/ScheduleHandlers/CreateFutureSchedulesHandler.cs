using Application.AppEntry;
using Application.AppEntry.Commands.Schedule;
using Domain.Aggregates.Schedule;
using OperationResult;

namespace Application.Handlers.ScheduleHandlers;

public class CreateFutureSchedulesHandler(IScheduleRepository scheduleRepository) : ICommandHandler<CreateFutureSchedulesCommand, None>
{
    private readonly IScheduleRepository _scheduleRepository = scheduleRepository;
    
    public async Task<Result<None>> HandleAsync(CreateFutureSchedulesCommand command)
    {
        var now = DateTime.Now;
        
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
        
        return Result<None>.Success(None.Value);
    }
}