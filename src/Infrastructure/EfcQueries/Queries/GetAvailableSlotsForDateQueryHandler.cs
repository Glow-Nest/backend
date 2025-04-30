using Domain.Aggregates.Schedule;
using Domain.Aggregates.Schedule.Values;
using Domain.Common;
using Domain.Common.OperationResult;
using EfcQueries.Models;
using Microsoft.EntityFrameworkCore;
using QueryContracts.Contracts;
using QueryContracts.Queries.Schedule;
using Schedule = EfcQueries.Models.Schedule;

namespace EfcQueries.Queries;

public class GetAvailableSlotsForDateQueryHandler(PostgresContext context)
    : IQueryHandler<GetAvailableSlotsForDate.Query, Result<GetAvailableSlotsForDate.Answer>>
{
    public async Task<Result<GetAvailableSlotsForDate.Answer>> HandleAsync(GetAvailableSlotsForDate.Query query)
    {
        if (!DateOnly.TryParse(query.ScheduleDate, out var scheduleDate))
        {
            return Result<GetAvailableSlotsForDate.Answer>.Fail(GenericErrorMessage.ErrorParsingDate());
        }

        var schedule = await context.Schedules
            .Include(s => s.Appointments)
            .Include(s => s.BlockedTimes)
            .FirstOrDefaultAsync(s => s.ScheduleDate == scheduleDate.ToString("yyyy-MM-dd"));

        if (schedule == null)
        {
            return Result<GetAvailableSlotsForDate.Answer>.Fail(ScheduleErrorMessage.ScheduleNotFound(scheduleDate));
        }

        var timeSlots = BuildAvailableTimeSlots(schedule);
        var answer = new GetAvailableSlotsForDate.Answer(timeSlots);

        return Result<GetAvailableSlotsForDate.Answer>.Success(answer);
    }

    private static Dictionary<string, List<TimeSlot>> BuildAvailableTimeSlots(Schedule schedule)
    {
        var result = new Dictionary<string, List<TimeSlot>>();

        var timePeriods = new Dictionary<string, (TimeOnly Start, TimeOnly End)>
        {
            ["Morning"] = (ScheduleBusinessHours.OpeningHour, new TimeOnly(12, 0)),
            ["Afternoon"] = (new TimeOnly(12, 30), new TimeOnly(17, 0)),
            ["Evening"] = (new TimeOnly(17, 0), ScheduleBusinessHours.ClosingHour)
        };

        foreach (var (label, (start, end)) in timePeriods)
        {
            result[label] = GenerateSlots(start, end, schedule);
        }

        return result;
    }

    private static List<TimeSlot> GenerateSlots(TimeOnly start, TimeOnly end, Schedule schedule)
    {
        var slots = new List<TimeSlot>();
        var current = start;

        while (current < end)
        {
            var maybeSlot = TimeSlot.Create(current, current.AddMinutes(30));
            if (!maybeSlot.IsSuccess)
            {
                current = current.AddMinutes(30);
                continue;
            }

            var slot = maybeSlot.Data;
            var isBlocked = schedule.BlockedTimes.Any(b => TimeSlot.Create(b.StartTime, b.EndTime).Data.Overlaps(slot));
            var isBooked = schedule.Appointments.Any(a => TimeSlot.Create(a.StartTime, a.EndTime).Data.Overlaps(slot));

            if (!isBlocked && !isBooked)
            {
                slots.Add(slot);
            }

            current = current.AddMinutes(30);
        }

        return slots;
    }
}
