using Application.AppEntry;
using Application.AppEntry.Commands.Appointment;
using Domain.Aggregates.Appointment;
using Domain.Aggregates.Appointment.Contracts;
using Domain.Aggregates.Client.Contracts;
using Domain.Common.OperationResult;

namespace Application.Handlers.AppointmentHandlers;

public class CreateAppointmentHandler : ICommandHandler<CreateAppointmentCommand>
{
    private readonly IServiceChecker _serviceChecker;
    private readonly IClientChecker _clientChecker;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IBlockedTimeChecker _blockedTimeChecker;

    private readonly IAppointmentRepository _appointmentRepository;

    public CreateAppointmentHandler(IServiceChecker serviceChecker, IClientChecker clientChecker,
        IDateTimeProvider dateTimeProvider, IBlockedTimeChecker blockedTimeChecker, IAppointmentRepository appointmentRepository)
    {
        _serviceChecker = serviceChecker;
        _clientChecker = clientChecker;
        _dateTimeProvider = dateTimeProvider;
        _blockedTimeChecker = blockedTimeChecker;
        _appointmentRepository = appointmentRepository;
    }

    public async Task<Result> HandleAsync(CreateAppointmentCommand command)
    {
        var createAppointmentDto = new CreateAppointmentDto(
            command.appointmentNote,
            command.timeSlot,
            command.appointmentDate,
            command.serviceIds,
            command.bookedByClient
        );

        var createAppointmentResult = await Appointment.Create(createAppointmentDto, _serviceChecker, _clientChecker, _dateTimeProvider,
            _blockedTimeChecker);
        if (!createAppointmentResult.IsSuccess)
        {
            return createAppointmentResult.ToNonGeneric();
        }

        var addResult = await _appointmentRepository.AddAsync(createAppointmentResult.Data);
        if (!addResult.IsSuccess)
        {
            return Result.Fail(addResult.Errors);
        }
        
        return Result.Success();
    }
}