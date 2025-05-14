using OperationResult;
using QueryContracts.Contracts;

namespace QueryContracts.Queries.Schedule;

public enum AppointmentTimeFrame
{
    PAST,
    FUTURE
}

public class GetClientsAppointment
{
    public record AppointmentDto(string AppointmentId, string AppointmentDate, TimeOnly StartTime, TimeOnly EndTime, string ClientName, List<string> Services, string AppointmentNote);

    public record Query(string ClientId,AppointmentTimeFrame TimeFrame) : IQuery<Result<Answer>>;
    public record Answer(List<AppointmentDto> Appointments);
}