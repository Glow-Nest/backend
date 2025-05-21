using Domain.Aggregates.Client.Values;
using Domain.Aggregates.Schedule;
using Domain.Aggregates.Schedule.Contracts;
using Domain.Aggregates.Schedule.Values;
using Domain.Aggregates.Schedule.Values.BlockedTimeValues;
using Domain.Aggregates.ServiceCategory.Values;
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
        _serviceCheckerMock.Setup(s => s.DoesServiceExistsAsync(It.IsAny<CategoryId>(), It.IsAny<ServiceId>())).ReturnsAsync(true);
        _clientCheckerMock.Setup(c => c.DoesClientExistsAsync(It.IsAny<ClientId>())).ReturnsAsync(true);
        _dateTimeProviderMock.Setup(d => d.GetNow()).Returns(DateTime.Now);
    }

    [Fact]
    public async Task Add_ShouldSucceed_WhenAllValidationsPass()
    {
        // Arrange
        var scheduleDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
        var schedule = Schedule.CreateSchedule(scheduleDate).Data;
        var timeSlot = TimeSlot.Create(TimeOnly.Parse("10:00"), TimeOnly.Parse("10:30"));
        var reason = BlockReason.Create("Test reason").Data;

        _dateTimeProviderMock.Setup(d => d.GetNow()).Returns(DateTime.Now.AddDays(-1));
        
        // Act
        var result = await schedule.AddBlockedTime(timeSlot.Data, reason, _dateTimeProviderMock.Object);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Data.BlockedTimeSlots);
    }

    [Fact]
    public async Task Add_ShouldFail_WhenTimeSlotOverlapsWithExistingBlockedTimeSlot()
    {
        // Arrange
        var scheduleDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
        var schedule = Schedule.CreateSchedule(scheduleDate).Data;
        var timeSlot1 = TimeSlot.Create(TimeOnly.Parse("10:00"), TimeOnly.Parse("10:30"));
        var timeSlot2 = TimeSlot.Create(TimeOnly.Parse("10:00"), TimeOnly.Parse("10:30"));
        var reason = BlockReason.Create("Test reason").Data;
        
        _dateTimeProviderMock.Setup(d => d.GetNow()).Returns(DateTime.Now.AddDays(-1));

        // Act
        var blockedTimeSlotResult1 = await schedule.AddBlockedTime(timeSlot1.Data, reason, _dateTimeProviderMock.Object);
        var result = await schedule.AddBlockedTime(timeSlot2.Data, reason, _dateTimeProviderMock.Object);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(ScheduleErrorMessage.BlockTimeSlotOverlap(), result.Errors);
    }

    [Fact]
    public async Task Add_ShouldFail_WhenTimeSlotOverlapsWithExistingAppointment()
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
        var blockedTimeSlotResult = await schedule.AddBlockedTime(timeSlot, reason, _dateTimeProviderMock.Object);

        // Assert
        Assert.False(blockedTimeSlotResult.IsSuccess);
        Assert.Contains(ScheduleErrorMessage.BlockTimeSlotOverlapsExistingAppointment(), blockedTimeSlotResult.Errors);
    }
}