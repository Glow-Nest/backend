using Domain.Aggregates.Appointment;
using Domain.Aggregates.Appointment.Contracts;
using Domain.Aggregates.Appointment.Values;
using Domain.Aggregates.Client.Contracts;
using Domain.Aggregates.Client.Values;
using Domain.Aggregates.Schedule.Entities;
using Domain.Aggregates.Schedule.Values;
using Domain.Aggregates.Schedule.Values.Appointment;
using Domain.Aggregates.ServiceCategory.Values;
using Domain.Common.Contracts;
using Domain.Common.OperationResult;
using Moq;

namespace UnitTest.Features.Helpers.Builders;

public class AppointmentBuilder
{
    private string _appointmentNote = "Valid appointment note";
    private TimeOnly _startTime = new TimeOnly(10, 0);
    private TimeOnly _endTime = new TimeOnly(11, 0);
    private DateOnly _appointmentDate = DateOnly.FromDateTime(DateTime.Now);
    private List<Guid> _serviceIds = [Guid.NewGuid()];
    private Guid _bookedByClient = Guid.NewGuid();

    private Mock<IServiceChecker> _serviceCheckerMock = new();
    private Mock<IClientChecker> _clientCheckerMock = new();
    private Mock<IDateTimeProvider> _dateTimeProviderMock = new();

    public static AppointmentBuilder CreateValid() => new();

    public async Task<Result<Appointment>> BuildAsync()
    {
        _dateTimeProviderMock.Setup(x => x.GetNow())
            .Returns(DateTime.Now.AddDays(-1));
        
        var serviceIds = new List<ServiceId>();

        var appointmentNoteResult = AppointmentNote.Create(_appointmentNote);
        var timeSlotResult = TimeSlot.Create(_startTime, _endTime);
        var clientId = ClientId.FromGuid(_bookedByClient);

        foreach (var serviceId in _serviceIds)
        {
            var id = ServiceId.FromGuid(serviceId);
            serviceIds.Add(id);
        }

        if (!appointmentNoteResult.IsSuccess)
        {
            return Result<Appointment>.Fail(appointmentNoteResult.Errors);
        }

        if (!timeSlotResult.IsSuccess)
        {
            return Result<Appointment>.Fail(timeSlotResult.Errors);
        }

        var createAppointmentDto = new CreateAppointmentDto(
            appointmentNoteResult.Data,
            timeSlotResult.Data,
            _appointmentDate,
            serviceIds,
            clientId
        );

        var result = await Appointment.Create(createAppointmentDto, _serviceCheckerMock.Object, _clientCheckerMock.Object, _dateTimeProviderMock.Object);
        
        if (!result.IsSuccess)
        {
            return Result<Appointment>.Fail(result.Errors);
        }
        
        return Result<Appointment>.Success(result.Data);
    }
    
    public Result<CreateAppointmentDto> BuildDto()
    {
        var appointmentNoteResult = AppointmentNote.Create(_appointmentNote);
        var timeSlotResult = TimeSlot.Create(_startTime, _endTime);
        var clientId = ClientId.FromGuid(_bookedByClient);

        var serviceIds = _serviceIds
            .Select(ServiceId.FromGuid)
            .ToList();

        if (!appointmentNoteResult.IsSuccess)
        {
            return Result<CreateAppointmentDto>.Fail(appointmentNoteResult.Errors);
        }

        if (!timeSlotResult.IsSuccess)
        {
            return Result<CreateAppointmentDto>.Fail(timeSlotResult.Errors);
        }

        var dto = new CreateAppointmentDto(
            appointmentNoteResult.Data,
            timeSlotResult.Data,
            _appointmentDate,
            serviceIds,
            clientId
        );

        return Result<CreateAppointmentDto>.Success(dto);
    }

    
    public AppointmentBuilder WithNote(string note)
    {
        _appointmentNote = note;
        return this;
    }
    
    public AppointmentBuilder WithStartTime(TimeOnly startTime)
    {
        _startTime = startTime;
        return this;
    }
    
    public AppointmentBuilder WithEndTime(TimeOnly endTime)
    {
        _endTime = endTime;
        return this;
    }
    
    public AppointmentBuilder WithAppointmentDate(DateOnly appointmentDate)
    {
        _appointmentDate = appointmentDate;
        return this;
    }
    
    public AppointmentBuilder WithServiceIds(List<Guid> serviceIds)
    {
        _serviceIds = serviceIds;
        return this;
    }
    
    public AppointmentBuilder WithBookedByClient(Guid bookedByClient)
    {
        _bookedByClient = bookedByClient;
        return this;
    }
    
    public AppointmentBuilder WithServiceCheckerMock(Mock<IServiceChecker> serviceCheckerMock)
    {
        _serviceCheckerMock = serviceCheckerMock;
        return this;
    }
    
    public AppointmentBuilder WithClientCheckerMock(Mock<IClientChecker> clientCheckerMock)
    {
        _clientCheckerMock = clientCheckerMock;
        return this;
    }
    
    public AppointmentBuilder WithDateTimeProviderMock(Mock<IDateTimeProvider> dateTimeProviderMock)
    {
        _dateTimeProviderMock = dateTimeProviderMock;
        return this;
    }
}