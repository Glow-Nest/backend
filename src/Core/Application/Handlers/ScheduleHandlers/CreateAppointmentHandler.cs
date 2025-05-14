using Application.AppEntry;
using Application.AppEntry.Commands.Schedule;
using Domain.Aggregates.Appointment.Contracts;
using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Values;
using Domain.Aggregates.Schedule;
using Domain.Aggregates.Schedule.Contracts;
using Domain.Aggregates.Schedule.Entities;
using Domain.Common.Contracts;
using OperationResult;

namespace Application.Handlers.ScheduleHandlers;

public class CreateAppointmentHandler(IServiceChecker serviceChecker, IClientChecker clientChecker, IDateTimeProvider dateTimeProvider, IScheduleRepository scheduleRepository, IClientRepository clientRepository) : ICommandHandler<CreateAppointmentCommand, None>
{
    public async Task<Result<None>> HandleAsync(CreateAppointmentCommand command)
    {
        var emailResult = await clientRepository.GetAsync(command.email);
        if (!emailResult.IsSuccess)
        {
            return emailResult.ToNonGeneric().ToNone();
        }
        
        var client = emailResult.Data;

        var dto = ToDto(command, client.ClientId);

        var scheduleResult = await scheduleRepository.GetScheduleByDateAsync(command.appointmentDate);
        if (!scheduleResult.IsSuccess)
        {
            return scheduleResult.ToNonGeneric().ToNone();
        }

        var schedule = scheduleResult.Data;

        var addResult = await schedule.AddAppointment(dto, serviceChecker, clientChecker, dateTimeProvider);
        if (!addResult.IsSuccess)
        {
            return addResult.ToNonGeneric().ToNone();
        }

        return Result<None>.Success(None.Value);
    }

    private static CreateAppointmentDto ToDto(CreateAppointmentCommand command, ClientId clientId)
    {
        return new CreateAppointmentDto(
            command.appointmentNote,
            command.timeSlot,
            command.appointmentDate,
            command.serviceIds,
            command.categoryIds,
            clientId
        );
    }
}