using Application.AppEntry.Commands.Schedule;
using Domain.Common;

namespace UnitTest.Features.ScheduleTest.AddBlockedTime;

public class AddBlockedTimeCommandTest
{
    [Fact]
    public void AddBlockedTimeCommand_ShouldBeValid_WhenAllPropertiesAreSet()
    {
        // Arrange
        var startTime = "10:00";
        var endTime = "11:00";

        var date = DateOnly.FromDateTime(DateTime.Today).AddDays(1).ToString();

        // Act
        var commandResult = AddBlockedTimeCommand.Create(startTime, endTime, date);

        // Assert
        Assert.True(commandResult.IsSuccess);
    }
    
    [Fact]
    public void AddBlockedTimeCommand_ShouldFail_WhenStartTimeIsInvalid()
    {
        // Arrange
        var startTime = "invalid_time";
        var endTime = "11:00";
        
        var date = DateOnly.FromDateTime(DateTime.Today).AddDays(1).ToString();

        // Act
        var commandResult = AddBlockedTimeCommand.Create(startTime, endTime, date);

        // Assert
        Assert.False(commandResult.IsSuccess);
        Assert.Contains(GenericErrorMessage.ErrorParsingTime(), commandResult.Errors);
    }
    
}