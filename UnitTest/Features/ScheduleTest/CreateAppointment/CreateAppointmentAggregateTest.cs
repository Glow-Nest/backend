using Domain.Aggregates.Appointment;
using Domain.Aggregates.Appointment.Contracts;
using Domain.Aggregates.Appointment.Values;
using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Values;
using Domain.Aggregates.Schedule;
using Domain.Aggregates.Schedule.Entities;
using Domain.Aggregates.Schedule.Values;
using Domain.Aggregates.Schedule.Values.BlockedTime;
using Domain.Aggregates.ServiceCategory;
using Domain.Aggregates.ServiceCategory.Values;
using Domain.Common.Contracts;
using Moq;
using UnitTest.Features.Helpers;

namespace UnitTest.Features.ScheduleTest.CreateAppointment;

public class CreateAppointmentAggregateTest
{
    private readonly Mock<IServiceChecker> _serviceCheckerMock = new();
    private readonly Mock<IClientChecker> _clientCheckerMock = new();
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock = new();
    private readonly Mock<IBlockedTimeChecker> _blockedTimeCheckerMock = new();

    private void SetupMocksForValidScenario()
    {
        _serviceCheckerMock.Setup(s => s.DoesServiceExistsAsync(It.IsAny<ServiceId>())).ReturnsAsync(true);
        _clientCheckerMock.Setup(c => c.DoesClientExistsAsync(It.IsAny<ClientId>())).ReturnsAsync(true);
        _dateTimeProviderMock.Setup(d => d.GetNow()).Returns(DateTime.Now);
        _blockedTimeCheckerMock.Setup(b => b.IsBlockedTimeAsync(It.IsAny<DateOnly>(), It.IsAny<TimeOnly>(), It.IsAny<TimeOnly>())).ReturnsAsync(false);
    }
    
    [Fact]
    public async Task Create_ShouldSucceed_WhenAllValidationsPass()
    {
        // Arrange
        SetupMocksForValidScenario();
        var appointmentDto = HelperMethods.CreateValidAppointmentDto();

        var schedule = Schedule.CreateSchedule(appointmentDto.BookingDate).Data;

        // Act
        var result = await schedule.AddAppointment(appointmentDto, _serviceCheckerMock.Object, _clientCheckerMock.Object, _dateTimeProviderMock.Object);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
        Assert.Single(schedule.Appointments);
    }

    [Fact]
    public async Task Create_ShouldFail_WhenServiceDoesNotExist()
    {
        // Arrange
        var appointmentDto = HelperMethods.CreateValidAppointmentDto();
        _serviceCheckerMock.Setup(s => s.DoesServiceExistsAsync(It.IsAny<ServiceId>())).ReturnsAsync(false);
        
        var schedule = Schedule.CreateSchedule(appointmentDto.BookingDate).Data;
        
        // Act
        var result = await schedule.AddAppointment(appointmentDto, _serviceCheckerMock.Object, _clientCheckerMock.Object, _dateTimeProviderMock.Object);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(ServiceCategoryErrorMessage.ServiceNotFound(), result.Errors);
        Assert.Empty(schedule.Appointments);
    }

    [Fact]
    public async Task Create_ShouldFail_WhenClientDoesNotExist()
    {
        // Arrange
        var appointmentDto = HelperMethods.CreateValidAppointmentDto();
        SetupMocksForValidScenario();
        _clientCheckerMock.Setup(c => c.DoesClientExistsAsync(It.IsAny<ClientId>())).ReturnsAsync(false);
        
        var schedule = Schedule.CreateSchedule(appointmentDto.BookingDate).Data;

        // Act
        var result = await schedule.AddAppointment(appointmentDto, _serviceCheckerMock.Object, _clientCheckerMock.Object, _dateTimeProviderMock.Object);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(ClientErrorMessage.ClientNotFound(), result.Errors);
        Assert.Empty(schedule.Appointments);
    }

    [Fact]
    public async Task Create_ShouldFail_WhenAppointmentIsInThePast()
    {
        // Arrange
        var appointmentDto = HelperMethods.CreateValidAppointmentDto();
        SetupMocksForValidScenario();
        _dateTimeProviderMock.Setup(d => d.GetNow()).Returns(DateTime.Now.AddDays(2));
        
        var schedule = Schedule.CreateSchedule(appointmentDto.BookingDate).Data;
        
        // Act
        var result = await schedule.AddAppointment(appointmentDto, _serviceCheckerMock.Object, _clientCheckerMock.Object, _dateTimeProviderMock.Object);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(ScheduleErrorMessage.AppointmentDateInPast(), result.Errors);
        Assert.Empty(schedule.Appointments);
    }

    [Fact]
    public async Task Create_Should_Fail_WhenAppointmentDateIsTooFar()
    {
        // Arrange
        var appointmentDto = HelperMethods.CreateValidAppointmentDto();
        SetupMocksForValidScenario();
        appointmentDto = appointmentDto with { BookingDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(3)) };

        var schedule = Schedule.CreateSchedule(appointmentDto.BookingDate).Data;
        
        // Act
        var result = await schedule.AddAppointment(appointmentDto, _serviceCheckerMock.Object, _clientCheckerMock.Object, _dateTimeProviderMock.Object);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(ScheduleErrorMessage.AppointmentDateTooFar(), result.Errors);
        Assert.Empty(schedule.Appointments);
    }
    
    [Fact]
    public async Task Create_ShouldFail_WhenBlockedTimeIsSelected()
    {
        // Arrange
        var appointmentDto = HelperMethods.CreateValidAppointmentDto();
        SetupMocksForValidScenario();
        
        _dateTimeProviderMock.Setup(d => d.GetNow()).Returns(DateTime.Now.AddDays(-1));
        
        var blockTimeSlot = TimeSlot.Create(appointmentDto.TimeSlot.Start, appointmentDto.TimeSlot.End).Data;
        var blockReason = BlockReason.Create("Test reason").Data;
        var schedule = Schedule.CreateSchedule(appointmentDto.BookingDate).Data;
        await schedule.AddBlockedTime(blockTimeSlot, blockReason, _dateTimeProviderMock.Object);

        // Act
        var result = await schedule.AddAppointment(appointmentDto, _serviceCheckerMock.Object, _clientCheckerMock.Object, _dateTimeProviderMock.Object);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(ScheduleErrorMessage.BlockedTimeSelected(), result.Errors);
        Assert.Empty(schedule.Appointments);
    }

    [Fact]
    public async Task Create_ShouldFail_WhenOutsideBusinessHours()
    {
        // Arrange
        var appointmentDto = HelperMethods.CreateValidAppointmentDto();
        SetupMocksForValidScenario();
        appointmentDto = appointmentDto with { TimeSlot = TimeSlot.Create(TimeOnly.Parse("08:00"), TimeOnly.Parse("09:00")).Data };
        
        var schedule = Schedule.CreateSchedule(appointmentDto.BookingDate).Data;
        
        // Act
        var result = await schedule.AddAppointment(appointmentDto, _serviceCheckerMock.Object, _clientCheckerMock.Object, _dateTimeProviderMock.Object);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(ScheduleErrorMessage.OutsideBusinessHours(), result.Errors);
        Assert.Empty(schedule.Appointments);
    }
}