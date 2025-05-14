using Application.AppEntry.Commands.ServiceCategory;
using Application.AppEntry.Commands.ServiceCategory.UpdateServiceCommand;
using Domain.Aggregates.ServiceCategory.Values;
using Domain.Common.Values;

namespace UnitTest.Features.ServiceCategoryTest.UpdateServicePriceTest;

public class UpdateServicePriceCommandTest
{
    [Fact]
    public void CreateServicePriceCommand_ShouldReturnSuccess_WhenServicePriceIsCreated()
    {
        // Arrange
        var categoryId = CategoryId.Create();
        var serviceId = ServiceId.Create();
        var servicePrice = Price.Create(100).Data;
        
        //Act
        var command = UpdateServicePriceCommand.Create(categoryId.Value.ToString(),serviceId.Value.ToString(),servicePrice.Value);

        // Assert
        Assert.True(command.IsSuccess);
    }
}