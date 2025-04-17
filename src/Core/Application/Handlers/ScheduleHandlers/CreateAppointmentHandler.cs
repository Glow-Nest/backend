using Application.AppEntry;
using Application.AppEntry.Commands.Schedule;
using Domain.Aggregates.Appointment.Contracts;
using Domain.Aggregates.Schedule;
using Domain.Aggregates.Schedule.Entities;
using Domain.Common.Contracts;
using Domain.Common.OperationResult;

namespace Application.Handlers.ScheduleHandlers;

public class CreateAppointmentHandler(IServiceChecker serviceChecker, IClientChecker clientChecker, IDateTimeProvider dateTimeProvider, IScheduleRepository scheduleRepository) : ICommandHandler<CreateAppointmentCommand>
{
    public async Task<Result> HandleAsync(CreateAppointmentCommand command)
    {
        var dto = ToDto(command);

        var scheduleResult = await GetOrCreateScheduleAsync(command.appointmentDate);
        if (!scheduleResult.IsSuccess)
        {
            return scheduleResult.ToNonGeneric();
        }

        var schedule = scheduleResult.Data;

        var addResult = await schedule.AddAppointment(dto, serviceChecker, clientChecker, dateTimeProvider);
        if (!addResult.IsSuccess)
        {
            return addResult.ToNonGeneric();
        }

        return Result.Success();
    }

    private static CreateAppointmentDto ToDto(CreateAppointmentCommand command)
    {
        return new CreateAppointmentDto(
            command.appointmentNote,
            command.timeSlot,
            command.appointmentDate,
            command.serviceIds,
            command.bookedByClient
        );
    }

    private async Task<Result<Schedule>> GetOrCreateScheduleAsync(DateOnly date)
    {
        var result = await scheduleRepository.GetScheduleByDateAsync(date);
        if (result.IsSuccess)
        {
            return result;
        }

        if (!result.HasError(ScheduleErrorMessage.ScheduleNotFound(date).ErrorId))
        {
            return result;
        }

        var newScheduleResult = Schedule.CreateSchedule(date);
        if (!newScheduleResult.IsSuccess)
        {
            return newScheduleResult;
        }

        await scheduleRepository.AddAsync(newScheduleResult.Data);
        return newScheduleResult;
    }
}