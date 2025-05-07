using Domain.Aggregates.Schedule;
using Domain.Aggregates.Schedule.Values.AppointmentValues;
using Domain.Common.OperationResult;
using EfcQueries.Models;
using Microsoft.EntityFrameworkCore;
using QueryContracts.Contracts;
using QueryContracts.Queries.Schedule;

namespace EfcQueries.Queries.Schedules;

public class GetClientAppointmentsQueryHandler(PostgresContext context) : IQueryHandler<GetClientsAppointment.Query, Result<GetClientsAppointment.Answer>>
{
    public async Task<Result<GetClientsAppointment.Answer>> HandleAsync(GetClientsAppointment.Query query)
    {
        var today = DateOnly.FromDateTime(DateTime.Now).ToString("O");
        var clientId = Guid.Parse(query.ClientId);
        
        var appointmentsQuery = context.Appointments
            .Include(a => a.Services)
            .Include(a => a.BookedByClientNavigation)
            .Where(a => a.AppointmentStatus != (int)AppointmentStatus.CANCELED && a.BookedByClient == clientId);

        appointmentsQuery = query.TimeFrame switch
        {
            AppointmentTimeFrame.PAST => appointmentsQuery
                .Where(a => a.AppointmentDate.CompareTo(today) < 0),

            AppointmentTimeFrame.FUTURE => appointmentsQuery
                .Where(a => a.AppointmentDate.CompareTo(today) > 0),

            _ => throw new ArgumentOutOfRangeException()
        };
        
        var appointments = appointmentsQuery.ToList();
        
        if (!appointments.Any())
        {
            return await Task.FromResult(Result<GetClientsAppointment.Answer>.Fail(ScheduleErrorMessage.NoAppointmentsFound()));
        }
        
        var answer = appointments.Select(a => new GetClientsAppointment.AppointmentDto(
            a.Id.ToString(),
            a.AppointmentDate,
            a.StartTime,
            a.EndTime,
            $"{a.BookedByClientNavigation.FirstName} {a.BookedByClientNavigation.LastName}",
            a.Services.Select(s => s.Name).ToList(),
            a.Note
        )).ToList();
        
        return Result<GetClientsAppointment.Answer>.Success(new GetClientsAppointment.Answer(answer));
    }
}