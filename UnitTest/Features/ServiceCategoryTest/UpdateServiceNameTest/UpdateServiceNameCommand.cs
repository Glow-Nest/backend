using Domain.Aggregates.ServiceCategory.Values;
using Domain.Common.Values;

namespace UnitTest.Features.ServiceCategoryTest.UpdateServiceNameTest;

public class UpdateServiceNameCommand
{
    [Fact]
    public void CreateServiceNameCommand_ShouldReturnSuccess_WhenServiceNameIsCreated()
    {
        // Arrange
        var categoryId = CategoryId.Create();
        var serviceId = ServiceId.Create();
        var serviceName = ServiceName.Create("service1").Data;
        var servicePrice = Price.Create(100).Data;
        TimeSpan serviceDuration = TimeSpan.FromMinutes(5);
        
        //Act
        var command = Application.AppEntry.Commands.ServiceCategory.UpdateServiceCommand.UpdateServiceNameCommand.Create(categoryId.Value.ToString(),serviceId.Value.ToString(),serviceName.Value);

        // Assert
        Assert.True(command.IsSuccess);
    }
}