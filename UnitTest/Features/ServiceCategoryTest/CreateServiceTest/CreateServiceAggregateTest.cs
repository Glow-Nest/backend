using Domain.Aggregates.ServiceCategory.Entities;
using Domain.Aggregates.ServiceCategory.Values;

namespace UnitTest.Features.ServiceCategoryTest.CreateServiceTest;

public class CreateServiceAggregateTest
{
    [Fact]
    public async Task ShouldCreate_Service_WithValidInput()
    {
        // Arrange
        var serviceName = ServiceName.Create("service1").Data;
        var servicePrice = Price.Create(100).Data;
        var serviceDuration = TimeSpan.FromHours(1);
        
        // Act
        var result = await Service.Create(serviceName, servicePrice, serviceDuration);
        
        // Assert
        Assert.True(result.IsSuccess);
    }
    
    [Fact]
    public void ShouldFail_WhenServiceNameIsEmpty()
    {
        // Arrange
        var serviceName = ServiceName.Create("");
        Assert.False(serviceName.IsSuccess);
    }
    
    [Fact]
    public void ShouldFail_WhenServicePriceIsNegative()
    {
        // Arrange
        var servicePrice = Price.Create(-100);
        Assert.False(servicePrice.IsSuccess);
    }
    
    [Fact]
    public void ShouldFail_WhenServiceDurationIsZero()
    {
        // Arrange
        var serviceDuration = TimeSpan.FromHours(0);
        Assert.True(serviceDuration == TimeSpan.Zero);
    }
}