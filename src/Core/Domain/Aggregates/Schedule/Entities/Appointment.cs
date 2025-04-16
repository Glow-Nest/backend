using Domain.Aggregates.Appointment;
using Domain.Aggregates.Appointment.Contracts;
using Domain.Aggregates.Appointment.Values;
using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Values;
using Domain.Aggregates.DailyAppointmentSchedule.Values.Appointment;
using Domain.Aggregates.Schedule.Values;
using Domain.Aggregates.Service;
using Domain.Aggregates.Service.Values;
using Domain.Common.BaseClasses;
using Domain.Common.Contracts;
using Domain.Common.OperationResult;

namespace Domain.Aggregates.Schedule.Entities;

public record CreateAppointmentDto(
    AppointmentNote Note,
    TimeSlot TimeSlot,
    DateOnly BookingDate,
    List<ServiceId> ServiceIds,
    ClientId BookedByClient
);

public class Appointment : Entity<AppointmentId>
{
    internal AppointmentStatus AppointmentStatus { get; }
    internal AppointmentNote AppointmentNote { get; }
    internal TimeSlot TimeSlot { get; }
    internal DateOnly AppointmentDate { get; }
    internal List<AppointmentServiceReference> Services { get; }
    internal ClientId BookedByClient { get; }

    public Appointment(AppointmentId id) : base(id)
    {
    }

    protected Appointment(AppointmentId appointmentId, AppointmentStatus appointmentStatus,
        AppointmentNote appointmentNote, TimeSlot timeSlot, DateOnly appointmentDate,
        List<AppointmentServiceReference> services, ClientId bookedByClient) : base(appointmentId)
    {
        AppointmentStatus = appointmentStatus;
        AppointmentNote = appointmentNote;
        TimeSlot = timeSlot;
        AppointmentDate = appointmentDate;
        Services = services;
        BookedByClient = bookedByClient;
    }

    public static async Task<Result<Appointment>> Create(CreateAppointmentDto appointmentDto,
        IServiceChecker serviceChecker,
        IClientChecker clientChecker, IDateTimeProvider dateTimeProvider)
    {
        var appointmentId = AppointmentId.Create();
        var status = AppointmentStatus.CREATED;

        // validate services
        var servicesResult = await ValidateServicesExist(appointmentDto.ServiceIds, serviceChecker);
        if (!servicesResult.IsSuccess) return servicesResult.ToGeneric<Appointment>();

        // validate client
        var clientExists = await clientChecker.DoesClientExistsAsync(appointmentDto.BookedByClient);
        if (!clientExists) return Result<Appointment>.Fail(ClientErrorMessage.ClientNotFound());

        // validate appointment date and time
        var timeValidation = ValidateAppointmentDateTime(appointmentDto, dateTimeProvider);
        if (!timeValidation.IsSuccess) return timeValidation.ToGeneric<Appointment>();

        var serviceReferences = appointmentDto.ServiceIds.Select(id => new AppointmentServiceReference(id)).ToList();

        var appointment = new Appointment(appointmentId, status, appointmentDto.Note, appointmentDto.TimeSlot,
            appointmentDto.BookingDate, serviceReferences, appointmentDto.BookedByClient);
        return Result<Appointment>.Success(appointment);
    }

    private static async Task<Result> ValidateServicesExist(List<ServiceId> serviceIds, IServiceChecker serviceChecker)
    {
        foreach (var serviceId in serviceIds)
        {
            if (!await serviceChecker.DoesServiceExistsAsync(serviceId))
            {
                return Result.Fail(ServiceErrorMessage.ServiceNotFound());
            }
        }

        return Result.Success();
    }

    private static Result ValidateAppointmentDateTime(CreateAppointmentDto appointmentDto,
        IDateTimeProvider dateTimeProvider)
    {
        var now = dateTimeProvider.GetNow();
        var today = DateOnly.FromDateTime(now);
        var currentTime = TimeOnly.FromDateTime(now);

        if (appointmentDto.BookingDate < today ||
            (appointmentDto.BookingDate == today && appointmentDto.TimeSlot.Start < currentTime))
        {
            return Result.Fail(ScheduleErrorMessage.AppointmentDateInPast());
        }

        if (appointmentDto.BookingDate >= today.AddMonths(3))
        {
            return Result.Fail(ScheduleErrorMessage.AppointmentDateTooFar());
        }

        if (appointmentDto.TimeSlot.Start < TimeOnly.Parse("09:00") ||
            appointmentDto.TimeSlot.End > TimeOnly.Parse("18:00"))
        {
            return Result.Fail(ScheduleErrorMessage.OutsideBusinessHours());
        }

        return Result.Success();
    }
}