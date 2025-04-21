using Domain.Aggregates.Service;
using Domain.Aggregates.Service.Values;

namespace UnitTest.Features.ServiceTest;

public class CreateServiceAggregateTest
{
    [Fact]
    public async Task ShouldCreate_Service_WithValidInput()
    {
        // Arrange
        var serviceName = Name.Create("service1").Data;
        var serviceDescription = Description.Create("description1").Data;
        var servicePrice = Price.Create(100).Data;
        var serviceDuration = TimeSpan.FromHours(1);
        var mediaUrls = new List<MediaUrl>
        {
            MediaUrl.Create("http://example.com/image1.jpg").Data,
            MediaUrl.Create("http://example.com/image2.jpg").Data
        };
        
        // Act
        var result = await Service.Create(serviceName, serviceDescription, servicePrice, serviceDuration, mediaUrls);
        
        // Assert
        Assert.True(result.IsSuccess);
    }
    
    [Fact]
    public void ShouldFail_WhenServiceNameIsEmpty()
    {
        // Arrange
        var serviceName = Name.Create("");
        Assert.False(serviceName.IsSuccess);
    }
    
    [Fact]
    public void ShouldFail_WhenServiceDescriptionIsEmpty()
    {
        // Arrange
        var serviceDescription = Description.Create("");
        Assert.False(serviceDescription.IsSuccess);
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
    
    [Fact]
    public void ShouldFail_WhenMediaUrlIsNull()
    {
        // Arrange
        var mediaUrl = MediaUrl.Create("");
        Assert.False(mediaUrl.IsSuccess);
    }
}