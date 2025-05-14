using Application.AppEntry;
using Application.AppEntry.Commands.Schedule;
using OperationResult;
using Quartz;

namespace Services.Jobs;

public class ScheduleSeederJob(ICommandDispatcher dispatcher)
{
    public async Task SeedFutureSchedulesAsync()
    {
        var command = new CreateFutureSchedulesCommand(30);
        await dispatcher.DispatchAsync<CreateFutureSchedulesCommand, None>(command);
    }
}