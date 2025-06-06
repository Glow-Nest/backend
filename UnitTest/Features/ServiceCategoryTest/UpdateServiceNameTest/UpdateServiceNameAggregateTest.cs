﻿using Domain.Aggregates.ServiceCategory.Entities;
using Domain.Aggregates.ServiceCategory.Values;
using Domain.Common.Values;

namespace UnitTest.Features.ServiceCategoryTest.UpdateServiceNameTest;

public class UpdateServiceNameAggregateTest
{
    [Fact]
    public async Task ShouldUpdate_ServiceName_WithValidInput()
    {
        // Arrange
        var serviceName = ServiceName.Create("service1").Data;
        var servicePrice = Price.Create(100).Data;
        var serviceDuration = TimeSpan.FromHours(1);
        
        var result = await Service.Create(serviceName, servicePrice, serviceDuration);
        var service = result.Data;
        
        var updateServiceName = ServiceName.Create("service2").Data;
        
        // Act
        var updateResult = service.UpdateServiceName(updateServiceName);
        
        // Assert
        Assert.True(updateResult.IsSuccess);
    }
}