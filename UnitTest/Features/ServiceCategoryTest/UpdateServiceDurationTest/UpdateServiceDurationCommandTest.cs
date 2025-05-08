using Domain.Aggregates.ServiceCategory.Values;

namespace UnitTest.Features.ServiceCategoryTest.UpdateServiceDurationTest;

public class UpdateServiceDurationCommandTest
{
    [Fact]
    public void CreateServiceNameCommand_ShouldReturnSuccess_WhenServiceNameIsCreated()
    {
        // Arrange
        var categoryId = CategoryId.Create();
        var serviceId = ServiceId.Create();
        TimeSpan serviceDuration = TimeSpan.FromMinutes(5);
        
        //Act
        var command = Application.AppEntry.Commands.ServiceCategory.UpdateServiceCommand.UpdateServiceDurationCommand.Create(categoryId.Value.ToString(),serviceId.Value.ToString(),serviceDuration);

        // Assert
        Assert.True(command.IsSuccess);
    }
}