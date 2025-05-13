using Application.AppEntry.Commands.ServiceCategory;
using Domain.Aggregates.ServiceCategory.Values;
using Domain.Common.Values;

namespace UnitTest.Features.ServiceCategoryTest.CreateServiceTest;

public class CreateServiceCommandTest
{
    [Fact]
    public void CreateServiceCommand_ShouldReturnSuccess_WhenServiceIsCreated()
    {
        // Arrange
        var categoryId = CategoryId.Create();
        var serviceName = ServiceName.Create("service1").Data;
        var servicePrice = Price.Create(100).Data;
        string serviceDuration = "1";
        
        //Act
        var command = AddServiceInCategoryCommand.Create(serviceName.Value,servicePrice.Value,serviceDuration,categoryId.Value.ToString());

        // Assert
        Assert.True(command.IsSuccess);
    }
}