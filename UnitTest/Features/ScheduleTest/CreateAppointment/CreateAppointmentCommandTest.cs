using Application.AppEntry.Commands.Schedule;
using Domain.Aggregates.Client;
using Domain.Aggregates.Schedule;
using Domain.Common;

namespace UnitTest.Features.ScheduleTest.CreateAppointment;

public class CreateAppointmentCommandTest
{
    [Fact]
    public void ShouldSucceed_WhenValidCommandsProvided()
    {
        // Arrange
        string note = "This is note!";
        string startTime = "10:00";
        string endTime = "11:00";
        string date = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
        List<string> serviceId = [Guid.NewGuid().ToString()];
        List<string> categoryId = [Guid.NewGuid().ToString()];
        string email = "test@gmail.com";
        
        // Act
        var command = CreateAppointmentCommand.Create(note, startTime, endTime, date, serviceId, categoryId, email);
        
        // Assert
        Assert.True(command.IsSuccess);
        Assert.NotNull(command.Data);
        Assert.Equal(note, command.Data.appointmentNote.Value);
    }
    
    [Fact]
    public void ShouldFail_WhenInvalidEmailProvided()
    {
        // Arrange
        string note = "This is note!";
        string startTime = "10:00";
        string endTime = "11:00";
        string date = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
        List<string> serviceId = [Guid.NewGuid().ToString()];
        List<string> categoryId = [Guid.NewGuid().ToString()];
        string clientEmail = "invalid-email";
        
        // Act
        var command = CreateAppointmentCommand.Create(note, startTime, endTime, date, categoryId, serviceId, clientEmail);
        
        // Assert
        Assert.False(command.IsSuccess);
        Assert.Contains(ClientErrorMessage.InvalidEmailFormat(), command.Errors);
    }
    
    [Fact]
    public void ShouldFail_WhenEmptyNoteProvided()
    {
        // Arrange
        string note = "";
        string startTime = "10:00";
        string endTime = "11:00";
        string date = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
        List<string> serviceId = [Guid.NewGuid().ToString()];
        List<string> categoryId = [Guid.NewGuid().ToString()];
        string clientId = Guid.NewGuid().ToString();
        
        // Act
        var command = CreateAppointmentCommand.Create(note, startTime, endTime, date, serviceId, categoryId, clientId);
        
        // Assert
        Assert.False(command.IsSuccess);
        Assert.Contains(ScheduleErrorMessage.EmptyAppointmentNote(), command.Errors);
    }
}