using Application.AppEntry.Commands.Service;
using Application.Handlers.ServiceHandlers;
using Domain.Aggregates.Service;
using Domain.Aggregates.Service.Values;
using Domain.Common.OperationResult;
using Moq;

namespace UnitTest.Features.ServiceTest;

public class CreateServiceHandlerTest
{
    private readonly Mock<IServiceRepository> _serviceRepositoryMock;
    private readonly CreateServiceHandler _handler;
    
    public CreateServiceHandlerTest()
    {
        _serviceRepositoryMock = new Mock<IServiceRepository>();
        _handler = new CreateServiceHandler(_serviceRepositoryMock.Object);
    }
    
    [Fact]
    public async Task HandleAsync_ShouldReturnSuccess_WhenServiceIsCreated()
    {
        // Arrange
        var serviceName = Name.Create("service1").Data;
        var serviceDescription = Description.Create("description1").Data;
        var servicePrice = Price.Create(100).Data;
        string serviceDuration = "1";
        List<string> mediaUrls = new List<string> { "mediaurl1", "mediaurl2" };
        
        var command = CreateServiceCommand.Create(serviceName.Value,serviceDescription.Value,servicePrice.Value,serviceDuration,mediaUrls);
        
        _serviceRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<Service>()))
            .ReturnsAsync(Result.Success());
        
        // Act
        var result = await _handler.HandleAsync(command.Data);
        
        // Assert
        Assert.True(result.IsSuccess);
    }
}