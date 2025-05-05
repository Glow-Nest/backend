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

public enum AppointmentMode
{
    SpecificDay,
    Past,
    Future
}

public class GetAppointmentsByDateQueryHandler(PostgresContext context) : IQueryHandler<GetAppointmentsByDate.Query, Result<GetAppointmentsByDate.Answer>>
{
    public async Task<Result<GetAppointmentsByDate.Answer>> HandleAsync(GetAppointmentsByDate.Query query)
    {
        if (!DateOnly.TryParse(query.ScheduleDate, out var scheduleDate))
        {
            return Result<GetAppointmentsByDate.Answer>.Fail(GenericErrorMessage.ErrorParsingDate());
        }

        if (!Enum.TryParse<AppointmentMode>(query.AppointmentMode, true, out var mode))
        {
            return Result<GetAppointmentsByDate.Answer>.Fail(ScheduleErrorMessage.InvalidAppointmentMode());
        }

        var appointmentQuery = context.Appointments
            .Include(a => a.Services)
            .Include(a => a.BookedByClientNavigation)
            .Where(a => a.AppointmentStatus != (int)AppointmentStatus.CANCELED);

        appointmentQuery = mode switch
        {
            AppointmentMode.SpecificDay => appointmentQuery
                .Where(a => a.AppointmentDate == query.ScheduleDate),

            AppointmentMode.Past => appointmentQuery
                .Where(a => a.AppointmentDate.CompareTo(query.ScheduleDate) < 0),

            AppointmentMode.Future => appointmentQuery
                .Where(a => a.AppointmentDate.CompareTo(query.ScheduleDate) > 0),

            _ => throw new ArgumentOutOfRangeException()
        };

        var appointments = await appointmentQuery.ToListAsync();
        
        if (!appointments.Any())
        {
            return Result<GetAppointmentsByDate.Answer>.Fail(ScheduleErrorMessage.NoAppointmentsFound());
        }
        
        var answer = appointments.Select(a => new GetAppointmentsByDate.AppointmentDto(
            a.Id.ToString(),
            a.AppointmentDate,
            a.StartTime,
            a.EndTime,
            $"{a.BookedByClientNavigation.FirstName} {a.BookedByClientNavigation.LastName}",
            a.Services.Select(s => s.Name).ToList(),
            a.Note
        )).ToList();
        
        return Result<GetAppointmentsByDate.Answer>.Success(new GetAppointmentsByDate.Answer(answer));    }
}