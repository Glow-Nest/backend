using Domain.Common.BaseClasses;
using OperationResult;

namespace Domain.Aggregates.Schedule.Values.AppointmentValues;

public class AppointmentNote : ValueObject
{
    public string Value { get; private set; }

    protected AppointmentNote(string value)
    {
        Value = value;
    }

    public static Result<AppointmentNote> Create(string appointmentNote)
    {
        if (string.IsNullOrWhiteSpace(appointmentNote))
        {
            return Result<AppointmentNote>.Fail(ScheduleErrorMessage.EmptyAppointmentNote());
        }
        
        var note = new AppointmentNote(appointmentNote);
        return Result<AppointmentNote>.Success(note);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}