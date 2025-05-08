using Application.AppEntry.Commands.Product;

namespace UnitTest.Features.ProductTest;

public class CreateProductCommandTest
{
    [Fact]
    public void CreateProductCommand_ShouldReturnSuccess_WhenProductIsCreated()
    {
        // Arrange
        var productName = "product1";
        var productPrice = 100;
        var productDescription = "description1";
        var imageUrl = "imageUrl";
        var inventoryCount = 10;

        // Act
        var command = CreateProductCommand.Create(productName, productPrice, productDescription, imageUrl, inventoryCount);

        // Assert
        Assert.True(command.IsSuccess);
    }
    
    [Fact]
    public void CreateProductCommand_ShouldReturnFailure_WhenProductNameIsEmpty()
    {
        // Arrange
        var productName = "";
        var productPrice = 100;
        var productDescription = "description1";
        var imageUrl = "imageUrl";
        var inventoryCount = 10;

        // Act
        var command = CreateProductCommand.Create(productName, productPrice, productDescription, imageUrl, inventoryCount);

        // Assert
        Assert.False(command.IsSuccess);
    }
}