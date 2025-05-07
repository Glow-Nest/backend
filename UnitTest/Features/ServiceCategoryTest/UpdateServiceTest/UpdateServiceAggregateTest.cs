using Domain.Aggregates.ServiceCategory.Entities;
using Domain.Aggregates.ServiceCategory.Values;

namespace UnitTest.Features.ServiceCategoryTest.UpdateServiceTest;

public class UpdateServiceAggregateTest
{
    [Fact]
    public async Task ShouldUpdate_Service_WithValidInput()
    {
        // Arrange
        var serviceName = ServiceName.Create("service1").Data;
        var servicePrice = Price.Create(100).Data;
        var serviceDuration = TimeSpan.FromHours(1);
        
        var result = await Service.Create(serviceName, servicePrice, serviceDuration);
        var service = result.Data;
        
        var updateServiceName = ServiceName.Create("service2").Data;
        var updateServicePrice = Price.Create(200).Data;
        var updateServiceDuration = TimeSpan.FromHours(2);
        
        // Act
        var updateResult = service.UpdateService(updateServiceName, updateServicePrice, updateServiceDuration);
        
        // Assert
        Assert.True(updateResult.IsSuccess);
    }
}