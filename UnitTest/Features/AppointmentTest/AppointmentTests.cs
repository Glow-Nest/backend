/*
using Domain.Aggregates.Appointment;
using Domain.Aggregates.Appointment.Contracts;
using Domain.Aggregates.Appointment.Values;
using Domain.Aggregates.Client.Contracts;
using Domain.Aggregates.Service;
using Moq;

namespace UnitTest.Features.AppointmentTest;

public class AppointmentTests
{
    private readonly Mock<IServiceChecker> _serviceCheckerMock = new();
    private readonly Mock<IClientChecker> _clientCheckerMock = new();
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock = new();
    private readonly Mock<IBlockedTimeChecker> _blockedTimeCheckerMock = new();

    [Fact]
    public async Task Create_ShouldSucceed_WhenAllValidationsPass()
    {
        // Arrange
        var appointmentDto = CreateValidAppointmentDto();
        SetupMocksForValidScenario();

        // Act
        var result = await Appointment.Create(appointmentDto, _serviceCheckerMock.Object, _clientCheckerMock.Object, _dateTimeProviderMock.Object, _blockedTimeCheckerMock.Object);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
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
        Assert.Equal(ServiceErrorMessage.ServiceNotFound(), result.ErrorMessage);
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
        Assert.Equal(ClientErrorMessage.ClientNotFound(), result.ErrorMessage);
    }

    [Fact]
    public async Task Create_ShouldFail_WhenAppointmentDateIsInThePast()
    {
        // Arrange
        var appointmentDto = CreateValidAppointmentDto();
        SetupMocksForValidScenario();
        _dateTimeProviderMock.Setup(d => d.GetNow()).Returns(DateTime.Now.AddDays(1)); // Simulate past date

        // Act
        var result = await Appointment.Create(appointmentDto, _serviceCheckerMock.Object, _clientCheckerMock.Object, _dateTimeProviderMock.Object, _blockedTimeCheckerMock.Object);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(AppointmentErrorMessage.AppointmentDateInPast(), result.ErrorMessage);
    }

    [Fact]
    public async Task Create_ShouldFail_WhenAppointmentDateIsTooFar()
    {
        // Arrange
        var appointmentDto = CreateValidAppointmentDto();
        SetupMocksForValidScenario();
        appointmentDto = appointmentDto with { BookingDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(4)) };

        // Act
        var result = await Appointment.Create(appointmentDto, _serviceCheckerMock.Object, _clientCheckerMock.Object, _dateTimeProviderMock.Object, _blockedTimeCheckerMock.Object);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(AppointmentErrorMessage.AppointmentDateTooFar(), result.ErrorMessage);
    }

    [Fact]
    public async Task Create_ShouldFail_WhenBlockedTimeIsSelected()
    {
        // Arrange
        var appointmentDto = CreateValidAppointmentDto();
        SetupMocksForValidScenario();
        _blockedTimeCheckerMock.Setup(b => b.IsBlockedTimeAsync(It.IsAny<DateOnly>(), It.IsAny<TimeOnly>(), It.IsAny<TimeOnly>())).ReturnsAsync(false);

        // Act
        var result = await Appointment.Create(appointmentDto, _serviceCheckerMock.Object, _clientCheckerMock.Object, _dateTimeProviderMock.Object, _blockedTimeCheckerMock.Object);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(AppointmentErrorMessage.BlockedTimeSelected(), result.ErrorMessage);
    }

    [Fact]
    public async Task Create_ShouldFail_WhenOutsideBusinessHours()
    {
        // Arrange
        var appointmentDto = CreateValidAppointmentDto();
        SetupMocksForValidScenario();
        appointmentDto = appointmentDto with { TimeSlot = new TimeSlot(TimeOnly.Parse("08:00"), TimeOnly.Parse("09:00")) };

        // Act
        var result = await Appointment.Create(appointmentDto, _serviceCheckerMock.Object, _clientCheckerMock.Object, _dateTimeProviderMock.Object, _blockedTimeCheckerMock.Object);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(AppointmentErrorMessage.OutsideBusinessHours(), result.ErrorMessage);
    }

    private CreateAppointmentDto CreateValidAppointmentDto()
    {
        return new CreateAppointmentDto(
            new AppointmentNote("Test Note"),
            new TimeSlot(TimeOnly.Parse("10:00"), TimeOnly.Parse("11:00")),
            DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
            new List<ServiceId> { new ServiceId(Guid.NewGuid()) },
            new ClientId(Guid.NewGuid())
        );
    }

    private void SetupMocksForValidScenario()
    {
        _serviceCheckerMock.Setup(s => s.DoesServiceExistsAsync(It.IsAny<ServiceId>())).ReturnsAsync(true);
        _clientCheckerMock.Setup(c => c.DoesClientExistsAsync(It.IsAny<ClientId>())).ReturnsAsync(true);
        _dateTimeProviderMock.Setup(d => d.GetNow()).Returns(DateTime.Now);
        _blockedTimeCheckerMock.Setup(b => b.IsBlockedTimeAsync(It.IsAny<DateOnly>(), It.IsAny<TimeOnly>(), It.IsAny<TimeOnly>())).ReturnsAsync(true);
    }
}
*/
