using Domain.Aggregates.Appointment;
using Domain.Aggregates.Appointment.Contracts;
using Domain.Aggregates.Appointment.Values;
using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Contracts;
using Domain.Aggregates.Client.Values;
using Domain.Aggregates.Service;
using Domain.Aggregates.Service.Values;
using Moq;

namespace UnitTest.Features.AppointmentTest.CreateAppointment;

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
    private CreateAppointmentDto CreateValidAppointmentDto()
    {
        return new CreateAppointmentDto(
            AppointmentNote.Create("Valid appointment note").Data,
            TimeSlot.Create(TimeOnly.Parse("10:00"), TimeOnly.Parse("11:00")).Data,
            DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
            [ServiceId.Create()],
            ClientId.Create()
        );
    }
    
    [Fact]
    public async Task Create_ShouldSucceed_WhenAllValidationsPass()
    {
        // Arrange
        SetupMocksForValidScenario();
        var appointmentDto = CreateValidAppointmentDto();
        
        // Act
        var result = await Appointment.Create(appointmentDto, _serviceCheckerMock.Object, _clientCheckerMock.Object, _dateTimeProviderMock.Object, _blockedTimeCheckerMock.Object);
        
        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
    }

    [Fact]
    public async Task Create_ShouldFail_WhenServiceDoesNotExist()
    {
        // Arrange
        var appointmentDto = CreateValidAppointmentDto();
        _serviceCheckerMock.Setup(s => s.DoesServiceExistsAsync(It.IsAny<ServiceId>())).ReturnsAsync(false);
        
        // Act
        var result = await Appointment.Create(appointmentDto, _serviceCheckerMock.Object, _clientCheckerMock.Object, _dateTimeProviderMock.Object, _blockedTimeCheckerMock.Object);
        
        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(ServiceErrorMessage.ServiceNotFound(), result.Errors);
    }

    [Fact]
    public async Task Create_ShouldFail_WhenClientDoesNotExist()
    {
        // Arrange
        var appointmentDto = CreateValidAppointmentDto();
        SetupMocksForValidScenario();
        _clientCheckerMock.Setup(c => c.DoesClientExistsAsync(It.IsAny<ClientId>())).ReturnsAsync(false);

        // Act
        var result = await Appointment.Create(appointmentDto, _serviceCheckerMock.Object, _clientCheckerMock.Object, _dateTimeProviderMock.Object, _blockedTimeCheckerMock.Object);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(ClientErrorMessage.ClientNotFound(), result.Errors);
    }

    [Fact]
    public async Task Create_ShouldFail_WhenAppointmentIsInThePast()
    {
        // Arrange
        var appointmentDto = CreateValidAppointmentDto();
        SetupMocksForValidScenario();
        _dateTimeProviderMock.Setup(d => d.GetNow()).Returns(DateTime.Now.AddDays(2));
        
        // Act
        var result = await Appointment.Create(appointmentDto, _serviceCheckerMock.Object, _clientCheckerMock.Object, _dateTimeProviderMock.Object, _blockedTimeCheckerMock.Object);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(AppointmentErrorMessage.AppointmentDateInPast(), result.Errors);
    }

    [Fact]
    public async Task Create_Should_Fail_WhenAppointmentDateIsTooFar()
    {
        // Arrange
        var appointmentDto = CreateValidAppointmentDto();
        SetupMocksForValidScenario();
        appointmentDto = appointmentDto with { BookingDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(3)) };

        // Act
        var result = await Appointment.Create(appointmentDto, _serviceCheckerMock.Object, _clientCheckerMock.Object, _dateTimeProviderMock.Object, _blockedTimeCheckerMock.Object);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(AppointmentErrorMessage.AppointmentDateTooFar(), result.Errors);
    }
    
    [Fact]
    public async Task Create_ShouldFail_WhenBlockedTimeIsSelected()
    {
        // Arrange
        var appointmentDto = CreateValidAppointmentDto();
        SetupMocksForValidScenario();
        _blockedTimeCheckerMock.Setup(b => b.IsBlockedTimeAsync(It.IsAny<DateOnly>(), It.IsAny<TimeOnly>(), It.IsAny<TimeOnly>())).ReturnsAsync(true);

        // Act
        var result = await Appointment.Create(appointmentDto, _serviceCheckerMock.Object, _clientCheckerMock.Object, _dateTimeProviderMock.Object, _blockedTimeCheckerMock.Object);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(AppointmentErrorMessage.BlockedTimeSelected(), result.Errors);
    }

    [Fact]
    public async Task Create_ShouldFail_WhenOutsideBusinessHours()
    {
        // Arrange
        var appointmentDto = CreateValidAppointmentDto();
        SetupMocksForValidScenario();
        appointmentDto = appointmentDto with { TimeSlot = TimeSlot.Create(TimeOnly.Parse("08:00"), TimeOnly.Parse("09:00")).Data };

        // Act
        var result = await Appointment.Create(appointmentDto, _serviceCheckerMock.Object, _clientCheckerMock.Object, _dateTimeProviderMock.Object, _blockedTimeCheckerMock.Object);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(AppointmentErrorMessage.OutsideBusinessHours(), result.Errors);
    }
}