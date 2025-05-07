using Domain.Common.OperationResult;

namespace Domain.Aggregates.Schedule;

public class ScheduleErrorMessage
{
    // Schedule Error
    public static Error ScheduleNotFound(DateOnly date) => new("Schedule.NotFound", $"Schedule for date {date:yyyy-MM-dd} was not found.");
    public static Error EndTimeStartError() => new("TimeSlot.EndTimeStart", "End time must be after start time.");
    public static Error ScheduleDateInPast() => new("Schedule.ScheduleDateInPast", "Schedule date must be in the future.");
    
    // Appointment Error
    public static Error AppointmentDateInPast() => new("Appointment.AppointmentInPast", "Appointment date must be in the future.");
    public static Error AppointmentDateTooFar() => new("Appointment.AppointmentTooFar", "Appointment date must be within 90 days.");
    public static Error OutsideBusinessHours() => new("Appointment.AppointmentOutsideBusinessHours", "Appointment date must be within business hours.");
    public static Error BlockedTimeSelected() => new("Appointment.BlockedTimeSelected", "Appointment time is blocked.");
    public static Error EmptyAppointmentNote() => new("Appointment.EmptyAppointmentNote", "Appointment note cannot be empty.");
    public static Error AppointmentOverlap() => new ("Appointment.Overlap", "Appointment time overlaps with another existing appointment.");
    public static Error NoAppointmentsFound() => new("Appointment.NoAppointmentsFound", "No appointments found for the selected date.");
    
    public static Error InvalidAppointmentMode() => new ("Appointment.InvalidAppointmentMode", "Invalid appointment mode. Valid values are 'Today', 'Past', or 'Future'.");
    public static Error InvalidAppointmentTimeFrame() => new ("Appointment.InvalidAppointmentTimeFrame", "Invalid appointment time frame. Valid values are 'Past' or 'Future'.");
    
    // Block Time Slot Error
    public static Error EmptyBlockReason() => new("BlockTimeSlot.EmptyBlockReason", "Blocked time slot reason cannot be empty.");
    public static Error BlockTimeSlotOverlap() => new("BlockTimeSlot.Overlap", "Blocked time slot overlaps with another existing blocked time slot.");
    public static Error BlockTimeSlotOverlapsExistingAppointment() => new("BlockTimeSlot.OverlapsExistingAppointment", "Blocked time slot overlaps with an existing appointment.");
    public static Error BlockedTimeInPast() => new("BlockedTime.BlockedTimeInPast", "Blocked time slot cannot be in the past.");
    public static Error BlockedTimeOutsideWorkingHours() => new("BlockedTime.OutsideWorkingHours", "Blocked time slot must be within working hours.");
}