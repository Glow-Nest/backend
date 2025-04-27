using Domain.Aggregates.Schedule;
using Domain.Common;
using Domain.Common.OperationResult;
using QueryContracts.Contracts;
using QueryContracts.Queries.Schedule;

namespace EfcQueries.Queries;

public class GetAvailableSlotsForDateQueryHandler(IScheduleRepository scheduleRepository)
    : IQueryHandler<GetAvailableSlotsForDate.Query, Result<GetAvailableSlotsForDate.Answer>>
{
    private readonly IScheduleRepository _scheduleRepository = scheduleRepository;

    public async Task<Result<GetAvailableSlotsForDate.Answer>> HandleAsync(GetAvailableSlotsForDate.Query query)
    {
        var scheduleDateParse = DateOnly.TryParse(query.ScheduleDate, out var scheduleDate);

        if (!scheduleDateParse)
        {
            return Result<GetAvailableSlotsForDate.Answer>.Fail(GenericErrorMessage.ErrorParsingDate());
        }

        var scheduleDateResult = await _scheduleRepository.GetScheduleByDateAsync(scheduleDate);
        if (!scheduleDateResult.IsSuccess)
        {
            return Result<GetAvailableSlotsForDate.Answer>.Fail(scheduleDateResult.Errors);
        }

        var schedule = scheduleDateResult.Data;
        var availableTimeSlotsResult = await schedule.GetAvailableTimeSlots();

        if (!availableTimeSlotsResult.IsSuccess)
        {
            return Result<GetAvailableSlotsForDate.Answer>.Fail(availableTimeSlotsResult.Errors);
        }

        var answer = new GetAvailableSlotsForDate.Answer(availableTimeSlotsResult.Data);
        return Result<GetAvailableSlotsForDate.Answer>.Success(answer);
    }
}