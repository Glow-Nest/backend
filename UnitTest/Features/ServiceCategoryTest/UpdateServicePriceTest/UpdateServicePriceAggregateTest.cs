using Domain.Aggregates.ServiceCategory.Entities;
using Domain.Aggregates.ServiceCategory.Values;

namespace UnitTest.Features.ServiceCategoryTest.UpdateServicePriceTest;

public class UpdateServicePriceAggregateTest
{
    [Fact]
    public async Task ShouldUpdate_ServicePrice_WithValidInput()
    {
        // Arrange
        var serviceName = ServiceName.Create("service1").Data;
        var servicePrice = Price.Create(100).Data;
        var serviceDuration = TimeSpan.FromHours(1);
        
        var result = await Service.Create(serviceName, servicePrice, serviceDuration);
        var service = result.Data;
        
        var updateServicePrice = Price.Create(122).Data;
        
        // Act
        var updateResult = service.UpdateServicePrice(updateServicePrice);
        
        // Assert
        Assert.True(updateResult.IsSuccess);
    }
}