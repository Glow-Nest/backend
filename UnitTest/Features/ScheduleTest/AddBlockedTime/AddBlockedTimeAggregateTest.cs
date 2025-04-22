using Domain.Aggregates.Appointment.Contracts;
using Domain.Aggregates.Client.Values;
using Domain.Aggregates.Schedule;
using Domain.Aggregates.Schedule.Contracts;
using Domain.Aggregates.Schedule.Values;
using Domain.Aggregates.Schedule.Values.BlockedTimeValues;
using Domain.Aggregates.Service.Values;
using Domain.Common.Contracts;
using Moq;
using UnitTest.Features.Helpers;

namespace UnitTest.Features.ScheduleTest.AddBlockedTime;

public class AddBlockedTimeAggregateTest
{
    private readonly Mock<IServiceChecker> _serviceCheckerMock = new();
    private readonly Mock<IClientChecker> _clientCheckerMock = new();
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock = new();

    private void SetupMocksForValidScenario()
    {
        _serviceCheckerMock.Setup(s => s.DoesServiceExistsAsync(It.IsAny<ServiceId>())).ReturnsAsync(true);
        _clientCheckerMock.Setup(c => c.DoesClientExistsAsync(It.IsAny<ClientId>())).ReturnsAsync(true);
        _dateTimeProviderMock.Setup(d => d.GetNow()).Returns(DateTime.Now);
    }

    [Fact]
    public void Add_ShouldSucceed_WhenAllValidationsPass()
    {
        // Arrange
        var scheduleDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
        var schedule = Schedule.CreateSchedule(scheduleDate).Data;
        var timeSlot = TimeSlot.Create(
            new TimeOnly(10, 0),
            new TimeOnly(11, 0)
        ).Data;
        
        var reason = BlockReason.Create("Test reason").Data;

        _dateTimeProviderMock.Setup(d => d.GetNow()).Returns(DateTime.Now.AddDays(-1));

        // Act
        var result = schedule.AddBlockedTime(timeSlot, reason, _dateTimeProviderMock.Object).Result;

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Data.BlockedTimeSlots);
    }

    [Fact]
    public async void Add_ShouldFail_WhenTimeSlotOverlapsWithExistingBlockedTimeSlot()
    {
        // Arrange
        var scheduleDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
        var schedule = Schedule.CreateSchedule(scheduleDate).Data;
        var timeSlot1 = TimeSlot.Create(TimeOnly.Parse("10:00"), TimeOnly.Parse("10:30"));
        var timeSlot2 = TimeSlot.Create(TimeOnly.Parse("10:00"), TimeOnly.Parse("10:30"));
        var reason = BlockReason.Create("Test reason").Data;

        _dateTimeProviderMock.Setup(d => d.GetNow()).Returns(DateTime.Now.AddDays(-1));

        // Act
        var blockedTimeSlotResult1 =
            await schedule.AddBlockedTime(timeSlot1.Data, reason, _dateTimeProviderMock.Object);
        var result = schedule.AddBlockedTime(timeSlot2.Data, reason, _dateTimeProviderMock.Object).Result;

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(ScheduleErrorMessage.BlockTimeSlotOverlap(), result.Errors);
    }

    [Fact]
    public async void Add_ShouldFail_WhenTimeSlotOverlapsWithExistingAppointment()
    {
        // Arrange
        SetupMocksForValidScenario();

        var scheduleDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
        var schedule = Schedule.CreateSchedule(scheduleDate).Data;

        var appointmentDto = HelperMethods.CreateValidAppointmentDto();
        var appointmentResult = await schedule.AddAppointment(appointmentDto, _serviceCheckerMock.Object,
            _clientCheckerMock.Object, _dateTimeProviderMock.Object);

        var timeSlot = TimeSlot.Create(appointmentDto.TimeSlot.Start, appointmentDto.TimeSlot.End).Data;
        var reason = BlockReason.Create("Test reason").Data;

        _dateTimeProviderMock.Setup(d => d.GetNow()).Returns(DateTime.Now.AddDays(-1));

        // Act
        var blockedTimeSlotResult = schedule.AddBlockedTime(timeSlot, reason, _dateTimeProviderMock.Object).Result;

        // Assert
        Assert.False(blockedTimeSlotResult.IsSuccess);
        Assert.Contains(ScheduleErrorMessage.BlockTimeSlotOverlapsExistingAppointment(), blockedTimeSlotResult.Errors);
    }
}