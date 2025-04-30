using Domain.Common.OperationResult;
using QueryContracts.Contracts;

namespace QueryContracts.Queries.Schedule;

public class GetAppointmentsForDate
{
    public record AppointmentDto(string AppointmentDate, TimeOnly StartTime, TimeOnly EndTime, string ClientName, List<string> Services);
    public record Query(string ScheduleDate) : IQuery<Result<Answer>>;
    public record Answer(List<AppointmentDto> Appointments);
}