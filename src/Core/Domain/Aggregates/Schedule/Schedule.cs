using Domain.Aggregates.Appointment.Contracts;
using Domain.Aggregates.Schedule.Contracts;
using Domain.Aggregates.Schedule.Entities;
using Domain.Aggregates.Schedule.Values;
using Domain.Aggregates.Schedule.Values.BlockedTimeValues;
using Domain.Common.BaseClasses;
using Domain.Common.Contracts;
using Domain.Common.OperationResult;

namespace Domain.Aggregates.Schedule;

public class Schedule : AggregateRoot
{
    internal ScheduleId ScheduleId { get; }
    internal List<Entities.Appointment> Appointments { get; } = new();
    internal List<BlockedTime> BlockedTimeSlots { get; } = new();
    internal DateOnly ScheduleDate { get; }


    public Schedule()
    {
    }

    protected Schedule(ScheduleId scheduleId, DateOnly scheduleDate)
    {
        ScheduleId = scheduleId;
        ScheduleDate = scheduleDate;
    }

    public static Result<Schedule> CreateSchedule(DateOnly scheduleDate)
    {
        var scheduleId = ScheduleId.Create();
        var schedule = new Schedule(scheduleId, scheduleDate);
        return Result<Schedule>.Success(schedule);
    }

    public async Task<Result<Schedule>> AddAppointment(CreateAppointmentDto appointmentDto,
        IServiceChecker serviceChecker,
        IClientChecker clientChecker, IDateTimeProvider dateTimeProvider)
    {
        var appointmentTimeSlot = appointmentDto.TimeSlot;

        // Check for blocked time slot conflict
        if (BlockedTimeSlots.Any(blocked => appointmentTimeSlot.Overlaps(blocked.TimeSlot)))
        {
            return Result<Schedule>.Fail(ScheduleErrorMessage.BlockedTimeSelected());
        }

        // Check for existing appointment conflict
        if (Appointments.Any(existing => existing.TimeSlot.Overlaps(appointmentTimeSlot)))
        {
            return Result<Schedule>.Fail(ScheduleErrorMessage.AppointmentOverlap());
        }

        var appointmentResult =
            await Entities.Appointment.Create(appointmentDto, serviceChecker, clientChecker, dateTimeProvider);

        if (!appointmentResult.IsSuccess)
        {
            return Result<Schedule>.Fail(appointmentResult.Errors);
        }

        Appointments.Add(appointmentResult.Data);
        return Result<Schedule>.Success(this);
    }

    public async Task<Result<Schedule>> AddBlockedTime(TimeSlot timeSlot, BlockReason reason,
        IDateTimeProvider dateTimeProvider)
    {
        // Check for existing blocked time slot conflict
        if (BlockedTimeSlots.Any(existing => existing.TimeSlot.Overlaps(timeSlot)))
        {
            return Result<Schedule>.Fail(ScheduleErrorMessage.BlockTimeSlotOverlap());
        }

        // Check for existing appointment conflict
        if (Appointments.Any(appointment => appointment.TimeSlot.Overlaps(timeSlot)))
        {
            return Result<Schedule>.Fail(ScheduleErrorMessage.BlockTimeSlotOverlapsExistingAppointment());
        }

        var blockedTimeResult = BlockedTime.Create(ScheduleDate, timeSlot, reason, dateTimeProvider);

        if (!blockedTimeResult.IsSuccess)
        {
            return Result<Schedule>.Fail(blockedTimeResult.Errors);
        }

        BlockedTimeSlots.Add(blockedTimeResult.Data);
        return Result<Schedule>.Success(this);
    }

    public async Task<Result<Dictionary<string, List<TimeSlot>>>> GetAvailableTimeSlots()
    {
        var timePeriods = new Dictionary<string, (TimeOnly Start, TimeOnly End)>
        {
            ["Morning"] = (ScheduleBusinessHours.OpeningHour, new TimeOnly(12, 0)),
            ["Afternoon"] = (new TimeOnly(12, 30), new TimeOnly(17, 0)),
            ["Evening"] = (new TimeOnly(17, 0), ScheduleBusinessHours.ClosingHour)
        };

        var timeSlots = new Dictionary<string, List<TimeSlot>>();

        foreach (var (label, (start, end)) in timePeriods)
        {
            var time = start;
            var timeSlotList = new List<TimeSlot>();

            while (time < end)
            {
                var timeSlot = TimeSlot.Create(time, time.AddMinutes(30)).Data;

                var isBlocked = BlockedTimeSlots.Any(blockedTime => blockedTime.TimeSlot.Overlaps(timeSlot));
                var isBooked = Appointments.Any(appointment => appointment.TimeSlot.Overlaps(timeSlot));

                if (!isBlocked && !isBooked)
                {
                    timeSlotList.Add(timeSlot);
                }

                time = time.AddMinutes(30);
            }

            timeSlots[label] = timeSlotList;
        }

        return Result<Dictionary<string, List<TimeSlot>>>.Success(timeSlots);
    }
}