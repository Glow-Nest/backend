using Application.AppEntry.Commands.ServiceCategory;
using Domain.Aggregates.ServiceCategory.Values;

namespace UnitTest.Features.ServiceCategoryTest.UpdateServiceTest;

public class UpdateServiceCommand
{
    [Fact]
    public void CreateServiceCommand_ShouldReturnSuccess_WhenServiceIsCreated()
    {
        // Arrange
        var categoryId = CategoryId.Create();
        var serviceId = ServiceId.Create();
        var serviceName = ServiceName.Create("service1").Data;
        var servicePrice = Price.Create(100).Data;
        TimeSpan serviceDuration = TimeSpan.FromMinutes(5);
        
        //Act
        var command = Application.AppEntry.Commands.ServiceCategory.UpdateServiceCommand.Create(categoryId.Value.ToString(),serviceId.Value.ToString(),serviceName.Value,servicePrice.Value,serviceDuration);

        // Assert
        Assert.True(command.IsSuccess);
    }
}