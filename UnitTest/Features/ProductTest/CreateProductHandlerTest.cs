using Application.AppEntry.Commands.Product;
using Application.Handlers.ProductHandlers;
using Domain.Aggregates.Product;
using Moq;
using OperationResult;

namespace UnitTest.Features.ProductTest;

public class CreateProductHandlerTest
{
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly CreateProductHandler _handler;
    
    public CreateProductHandlerTest()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _handler = new CreateProductHandler(_mockProductRepository.Object);
    }
    
    [Fact]
    public async Task HandleAsync_ShouldReturnSuccess_WhenProductIsCreated()
    {
        // Arrange
        var productName = "product1";
        var productPrice = 100;
        var productDescription = "description1";
        var imageUrl = "imageUrl";
        var inventoryCount = 10;

        var command = CreateProductCommand.Create(productName, productPrice, productDescription, imageUrl, inventoryCount);
        
        _mockProductRepository
            .Setup(repo => repo.AddAsync(It.IsAny<Product>()))
            .ReturnsAsync(Result.Success());
        
        // Act
        var result = await _handler.HandleAsync(command.Data);
        
        // Assert
        Assert.True(result.IsSuccess);
    }
}