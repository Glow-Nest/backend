using Domain.Aggregates.Appointment.Contracts;
using Domain.Aggregates.Schedule.Entities;
using Domain.Aggregates.Schedule.Values;
using Domain.Common.BaseClasses;
using Domain.Common.Contracts;
using Domain.Common.OperationResult;

namespace Domain.Aggregates.Schedule;

public class Schedule : AggregateRoot
{
    internal ScheduleId ScheduleId { get;}
    internal List<Entities.Appointment> Appointments { get; }
    internal List<TimeSlot> BlockedTimeSlots { get; }
    internal DateOnly ScheduleDate { get; }

    public Schedule()
    {
    }

    protected Schedule(ScheduleId scheduleId, DateOnly scheduleDate)
    {
        ScheduleId = scheduleId;
        Appointments = new ();
        BlockedTimeSlots = new ();
        ScheduleDate = scheduleDate;
    }

    public static Result<Schedule> CreateSchedule(DateOnly scheduleDate)
    {
        var scheduleId = ScheduleId.Create();
        var schedule = new Schedule(scheduleId, scheduleDate);
        return Result<Schedule>.Success(schedule);
    }

    public async Task<Result<Schedule>> AddAppointment(CreateAppointmentDto appointmentDto, IServiceChecker serviceChecker,
        IClientChecker clientChecker, IDateTimeProvider dateTimeProvider)
    {
        var appointmentTimeSlot = appointmentDto.TimeSlot;

        // Check for blocked time slot conflict
        if (BlockedTimeSlots.Any(blocked => appointmentTimeSlot.Overlaps(blocked)))
        {
            return Result<Schedule>.Fail(ScheduleErrorMessage.BlockedTimeSelected());
        }
        
        if (Appointments.Any(existing => existing.TimeSlot.Overlaps(appointmentTimeSlot)))
        {
            return Result<Schedule>.Fail(ScheduleErrorMessage.AppointmentOverlap());
        }

        var appointmentResult = await Entities.Appointment.Create(appointmentDto, serviceChecker, clientChecker, dateTimeProvider);

        if (!appointmentResult.IsSuccess)
        {
            return Result<Schedule>.Fail(appointmentResult.Errors);
        }
        
        Appointments.Add(appointmentResult.Data);
        return Result<Schedule>.Success(this);
    }

    public async Task<Result<Schedule>> AddBlockedTime(TimeSlot timeSlot)
    {
        // Check for existing blocked time slot conflict
        if (BlockedTimeSlots.Any(existing => existing.Overlaps(timeSlot)))
        {
            return Result<Schedule>.Fail(ScheduleErrorMessage.BlockTimeSlotOverlap());
        }
        
        // Check for existing appointment conflict
        if (Appointments.Any(appointment => appointment.TimeSlot.Overlaps(timeSlot)))
        {
            return Result<Schedule>.Fail(ScheduleErrorMessage.BlockTimeSlotOverlapsExistingAppointment());
        }
        
        BlockedTimeSlots.Add(timeSlot);
        return Result<Schedule>.Success(this);
    }
}