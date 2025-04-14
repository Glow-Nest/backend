using Domain.Common.OperationResult;

namespace Domain.Aggregates.Appointment;

public class AppointmentErrorMessage
{
    public static Error EndTimeStartError() => new("Appointment.EndTimeStart", "End time must be after start time.");
    public static Error AppointmentNotFound() => new("Appointment.AppointmentNotFound", "Appointment not found.");
    public static Error AppointmentDateInPast() => new("Appointment.AppointmentInPast", "Appointment date must be in the future.");
    public static Error AppointmentDateTooFar() => new("Appointment.AppointmentTooFar", "Appointment date must be within 90 days.");
    public static Error OutsideBusinessHours() => new("Appointment.AppointmentOutsideBusinessHours", "Appointment date must be within business hours.");
    public static Error BlockedTimeSelected() => new("Appointment.BlockedTimeSelected", "Appointment time is blocked.");
}