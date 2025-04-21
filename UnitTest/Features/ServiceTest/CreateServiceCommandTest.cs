using Application.AppEntry.Commands.Service;
using Domain.Aggregates.Service.Values;

namespace UnitTest.Features.ServiceTest;

public class CreateServiceCommandTest
{
    [Fact]
    public void CreateServiceCommand_ShouldReturnSuccess_WhenServiceIsCreated()
    {
        // Arrange
        var serviceName = Name.Create("service1").Data;
        var serviceDescription = Description.Create("description1").Data;
        var servicePrice = Price.Create(100).Data;
        string serviceDuration = "1";
        List<string> mediaUrls = new List<string> { "mediaurl1", "mediaurl2" };
        
        //Act
        var command = CreateServiceCommand.Create(serviceName.Value,serviceDescription.Value,servicePrice.Value,serviceDuration,mediaUrls);

        // Assert
        Assert.True(command.IsSuccess);
    }
}