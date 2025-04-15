using Domain.Aggregates.Appointment.Contracts;
using Domain.Aggregates.Appointment.Values;
using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Contracts;
using Domain.Aggregates.Client.Values;
using Domain.Aggregates.Service;
using Domain.Aggregates.Service.Values;
using Domain.Common.BaseClasses;
using Domain.Common.OperationResult;

namespace Domain.Aggregates.Appointment;

public record CreateAppointmentDto(
    AppointmentNote Note,
    TimeSlot TimeSlot,
    DateOnly BookingDate,
    List<ServiceId> ServiceIds,
    ClientId BookedByClient
);

public class Appointment : AggregateRoot
{
    internal AppointmentId AppointmentId { get; }
    internal AppointmentStatus AppointmentStatus { get; }
    internal AppointmentNote AppointmentNote { get; }
    internal TimeSlot TimeSlot { get; }
    internal DateOnly AppointmentDate { get; }
    internal List<ServiceId> Services { get; }
    internal ClientId BookedByClient { get; }
    
    // navigation for EF Core
    internal List<AppointmentService> AppointmentServices { get; private set; } = new();

    public Appointment() // for EFC
    {
    }

    protected Appointment(AppointmentId appointmentId, AppointmentStatus appointmentStatus, AppointmentNote appointmentNote, 
        TimeSlot timeSlot, DateOnly appointmentDate, List<ServiceId> services, ClientId bookedByClient)
    {
        AppointmentId = appointmentId;
        AppointmentStatus = appointmentStatus;
        AppointmentNote = appointmentNote;
        TimeSlot = timeSlot;
        AppointmentDate = appointmentDate;
        Services = services;
        BookedByClient = bookedByClient;
    }

    public static async Task<Result<Appointment>> Create(CreateAppointmentDto appointmentDto, IServiceChecker serviceChecker,
        IClientChecker clientChecker, IDateTimeProvider dateTimeProvider, IBlockedTimeChecker blockedTimeChecker)
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
        var timeValidation = await ValidateAppointmentDateTime(appointmentDto, dateTimeProvider, blockedTimeChecker);
        if (!timeValidation.IsSuccess) return timeValidation.ToGeneric<Appointment>();

        var appointment = new Appointment(appointmentId, status, appointmentDto.Note, appointmentDto.TimeSlot,
            appointmentDto.BookingDate, appointmentDto.ServiceIds, appointmentDto.BookedByClient);
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

    private static async Task<Result> ValidateAppointmentDateTime(CreateAppointmentDto appointmentDto, IDateTimeProvider dateTimeProvider, IBlockedTimeChecker blockedTimeChecker)
    {
        var now = dateTimeProvider.GetNow();
        var today = DateOnly.FromDateTime(now);
        var currentTime = TimeOnly.FromDateTime(now);

        if (appointmentDto.BookingDate < today ||
            (appointmentDto.BookingDate == today && appointmentDto.TimeSlot.Start < currentTime))
        {
            return Result.Fail(AppointmentErrorMessage.AppointmentDateInPast());
        }

        if (appointmentDto.BookingDate >= today.AddMonths(3))
        {
            return Result.Fail(AppointmentErrorMessage.AppointmentDateTooFar());
        }

        var isBlockedTime = await blockedTimeChecker.IsBlockedTimeAsync(appointmentDto.BookingDate, appointmentDto.TimeSlot.Start, appointmentDto.TimeSlot.End);
        if (isBlockedTime)
        {
            return Result.Fail(AppointmentErrorMessage.BlockedTimeSelected());
        }

        if (appointmentDto.TimeSlot.Start < TimeOnly.Parse("09:00") || appointmentDto.TimeSlot.End > TimeOnly.Parse("18:00"))
        {
            return Result.Fail(AppointmentErrorMessage.OutsideBusinessHours());
        }

        return Result.Success();
    }
}