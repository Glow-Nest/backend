using Domain.Aggregates.Client;
using Domain.Aggregates.Schedule;
using Domain.Aggregates.Schedule.Values.AppointmentValues;
using Domain.Common;
using Domain.Common.OperationResult;
using EfcQueries.Models;
using Microsoft.EntityFrameworkCore;
using QueryContracts.Contracts;
using QueryContracts.Queries.Schedule;

namespace EfcQueries.Queries;

public class GetAppointmentsForDateQueryHandler(PostgresContext context)
    : IQueryHandler<GetAppointmentsForDate.Query, Result<GetAppointmentsForDate.Answer>>
{
    public async Task<Result<GetAppointmentsForDate.Answer>> HandleAsync(GetAppointmentsForDate.Query query)
    {
        if (!DateOnly.TryParse(query.ScheduleDate, out var scheduleDate))
        {
            return Result<GetAppointmentsForDate.Answer>.Fail(GenericErrorMessage.ErrorParsingDate());
        }

        var schedule = await context.Schedules
            .Include(s => s.Appointments.Where(a => a.AppointmentStatus != (int)AppointmentStatus.CANCELED))
            .ThenInclude(a => a.Services)
            .Include(s => s.Appointments.Where(a => a.AppointmentStatus != (int)AppointmentStatus.CANCELED))
            .ThenInclude(a => a.BookedByClientNavigation)
            .FirstOrDefaultAsync(s => s.ScheduleDate == scheduleDate.ToString("yyyy-MM-dd"));
        
        if (schedule == null)
        {
            return Result<GetAppointmentsForDate.Answer>.Fail(ScheduleErrorMessage.ScheduleNotFound(scheduleDate));
        }

        var appointments = schedule.Appointments.Select(a => new GetAppointmentsForDate.AppointmentDto(
            a.AppointmentDate,
            a.StartTime,
            a.EndTime,
            $"{a.BookedByClientNavigation.FirstName} {a.BookedByClientNavigation.LastName}",
            a.Services.Select(s => s.Name).ToList()
        )).ToList();

        if (!appointments.Any())
        {
            return Result<GetAppointmentsForDate.Answer>.Fail(ScheduleErrorMessage.NoAppointmentsFound());
        }

        return Result<GetAppointmentsForDate.Answer>.Success(new GetAppointmentsForDate.Answer(appointments));
    }
}