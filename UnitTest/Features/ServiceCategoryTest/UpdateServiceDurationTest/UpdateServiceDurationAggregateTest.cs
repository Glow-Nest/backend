using Domain.Aggregates.ServiceCategory.Entities;
using Domain.Aggregates.ServiceCategory.Values;
using Domain.Common.Values;

namespace UnitTest.Features.ServiceCategoryTest.UpdateServiceDurationTest;

public class UpdateServiceDurationAggregateTest
{
    [Fact]
    public async Task ShouldUpdate_ServiceDuration_WithValidInput()
    {
        // Arrange
        var serviceName = ServiceName.Create("service1").Data;
        var servicePrice = Price.Create(100).Data;
        var serviceDuration = TimeSpan.FromHours(1);
        
        var result = await Service.Create(serviceName, servicePrice, serviceDuration);
        var service = result.Data;

        var updateServiceDuration = TimeSpan.FromHours(5);
        
        // Act
        var updateResult = service.UpdateServiceDuration(updateServiceDuration);
        
        // Assert
        Assert.True(updateResult.IsSuccess);
    }
}