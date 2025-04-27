using Application.AppEntry;
using Application.AppEntry.Commands.Schedule;
using Domain.Aggregates.Appointment.Contracts;
using Domain.Aggregates.Schedule;
using Domain.Aggregates.Schedule.Contracts;
using Domain.Aggregates.Schedule.Entities;
using Domain.Common.Contracts;
using Domain.Common.OperationResult;

namespace Application.Handlers.ScheduleHandlers;

public class CreateAppointmentHandler(IServiceChecker serviceChecker, IClientChecker clientChecker, IDateTimeProvider dateTimeProvider, IScheduleRepository scheduleRepository) : ICommandHandler<CreateAppointmentCommand>
{
    public async Task<Result> HandleAsync(CreateAppointmentCommand command)
    {
        var dto = ToDto(command);

        var scheduleResult = await scheduleRepository.GetScheduleByDateAsync(command.appointmentDate);
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
            command.categoryIds,
            command.bookedByClient
        );
    }
}