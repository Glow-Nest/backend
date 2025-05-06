using Domain.Common.OperationResult;
using QueryContracts.Contracts;

namespace QueryContracts.Queries.Schedule;

public class GetAppointmentsByDate
{
    public record AppointmentDto(string AppointmentId, string AppointmentDate, TimeOnly StartTime, TimeOnly EndTime, string ClientName, List<string> Services, string AppointmentNote);
    public record Query(string ScheduleDate, string AppointmentMode) : IQuery<Result<Answer>>;
    public record Answer(List<AppointmentDto> Appointments);
}