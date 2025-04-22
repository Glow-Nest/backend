using Application.AppEntry.Commands.Schedule;
using Application.Handlers.ScheduleHandlers;
using Domain.Aggregates.Appointment.Contracts;
using Domain.Aggregates.Client.Values;
using Domain.Aggregates.Schedule;
using Domain.Aggregates.Schedule.Contracts;
using Domain.Aggregates.Schedule.Values;
using Domain.Aggregates.Schedule.Values.AppointmentValues;
using Domain.Aggregates.Service.Values;
using Domain.Common.Contracts;
using Domain.Common.OperationResult;
using Moq;

namespace UnitTest.Features.ScheduleTest.CreateAppointment;

public class CreateAppointmentHandlerTest
{
    private readonly Mock<IServiceChecker> _serviceChecker = new();
    private readonly Mock<IClientChecker> _clientChecker = new();
    private readonly Mock<IDateTimeProvider> _dateTimeProvider = new();
    private readonly Mock<IScheduleRepository> _scheduleRepository = new();
    
    [Fact]
    public async Task ShouldSucceed_WhenValidCommandProvided()
    {
        // Arrange
        var bookingDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
        var command = new CreateAppointmentCommand(
            AppointmentNote.Create("Valid Note").Data,
            TimeSlot.Create(TimeOnly.Parse("10:00"), TimeOnly.Parse("11:00")).Data,
            [ServiceId.Create()],
            ClientId.Create(),
            bookingDate
        );

        var schedule = Schedule.CreateSchedule(bookingDate).Data;

        _serviceChecker.Setup(s => s.DoesServiceExistsAsync(It.IsAny<ServiceId>())).ReturnsAsync(true);
        _clientChecker.Setup(c => c.DoesClientExistsAsync(It.IsAny<ClientId>())).ReturnsAsync(true);
        _dateTimeProvider.Setup(d => d.GetNow()).Returns(DateTime.Now);
        _scheduleRepository.Setup(r => r.GetScheduleByDateAsync(It.IsAny<DateOnly>()))
            .ReturnsAsync(Result<Schedule>.Success(schedule));

        var appointmentHandler = new CreateAppointmentHandler(
            _serviceChecker.Object,
            _clientChecker.Object,
            _dateTimeProvider.Object,
            _scheduleRepository.Object
        );

        // Act
        var handleResult = await appointmentHandler.HandleAsync(command);

        // Assert
        Assert.True(handleResult.IsSuccess);
        _scheduleRepository.Verify(repo => repo.AddAsync(It.IsAny<Schedule>()), Times.Never);
    }
}