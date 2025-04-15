using Application.AppEntry.Commands.Appointment;
using Application.Handlers.AppointmentHandlers;
using Domain.Aggregates.Appointment;
using Domain.Aggregates.Appointment.Contracts;
using Domain.Aggregates.Appointment.Values;
using Domain.Aggregates.Client.Contracts;
using Domain.Aggregates.Client.Values;
using Domain.Aggregates.Service.Values;
using Domain.Common.OperationResult;
using Moq;

namespace UnitTest.Features.AppointmentTest.CreateAppointment;

public class CreateAppointmentHandlerTest
{
    private readonly Mock<IServiceChecker> _serviceChecker = new();
    private readonly Mock<IClientChecker> _clientChecker = new();
    private readonly Mock<IDateTimeProvider> _dateTimeProvider = new();
    private readonly Mock<IBlockedTimeChecker> _blockedTimeChecker = new();
    private readonly Mock<IAppointmentRepository> _appointmentRepository = new();
    
    [Fact]
    public async Task ShouldSucceed_WhenValidCommandProvided()
    {
        // Arrange
        var command = new CreateAppointmentCommand(
            AppointmentNote.Create("Valid Note").Data,
            TimeSlot.Create(TimeOnly.Parse("10:00"), TimeOnly.Parse("11:00")).Data,
            [ServiceId.Create()],
            ClientId.Create(),
            DateOnly.FromDateTime(DateTime.Now.AddDays(1))
        );
        
        _serviceChecker.Setup(s => s.DoesServiceExistsAsync(It.IsAny<ServiceId>())).ReturnsAsync(true);
        _clientChecker.Setup(c => c.DoesClientExistsAsync(It.IsAny<ClientId>())).ReturnsAsync(true);
        _dateTimeProvider.Setup(d => d.GetNow()).Returns(DateTime.Now);
        _blockedTimeChecker.Setup(b => b.IsBlockedTimeAsync(It.IsAny<DateOnly>(), It.IsAny<TimeOnly>(), It.IsAny<TimeOnly>())).ReturnsAsync(false);
        _appointmentRepository.Setup(r => r.AddAsync(It.IsAny<Appointment>()))
            .ReturnsAsync(Result.Success());

        var appointmentHandler = new CreateAppointmentHandler(
            _serviceChecker.Object,
            _clientChecker.Object,
            _dateTimeProvider.Object,
            _blockedTimeChecker.Object,
            _appointmentRepository.Object
        );

        // Act
        var handleResult = await appointmentHandler.HandleAsync(command);

        // Assert
        Assert.True(handleResult.IsSuccess);
        _appointmentRepository.Verify(repo => repo.AddAsync(It.IsAny<Appointment>()), Times.Once);
    }
}