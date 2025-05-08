using Domain.Aggregates.Product;
using Domain.Aggregates.Product.Values;

namespace UnitTest.Features.ProductTest;

public class CreateProductAggregateTest
{
    [Fact]
    public async Task ShouldCreate_Product_WithValidInput()
    {
        // Arrange
        var productName = Name.Create("product1").Data;
        var productPrice = Price.Create(100).Data;
        var productDescription = Description.Create("description1").Data;
        var imageUrl = ImageUrl.Create("imageUrl").Data;
        var inventoryCount = InventoryCount.Create(10).Data;
        
        // Act
        var result = await Product.Create(productName, productPrice,imageUrl, productDescription, inventoryCount);
        
        // Assert
        Assert.True(result.IsSuccess);
    }
    
    [Fact]
    public void ShouldFail_WhenProductNameIsEmpty()
    {
        // Arrange
        var name = Name.Create("");
        Assert.False(name.IsSuccess);
    }
    
    [Fact]
    public void ShouldFail_WhenProductDescriptionIsEmpty()
    {
        // Arrange
        var description = Description.Create("");
        Assert.False(description.IsSuccess);
    }
    
    [Fact]
    public void ShouldFail_WhenImageUrlIsEmpty()
    {
        // Arrange
        var imageUrl = ImageUrl.Create("");
        Assert.False(imageUrl.IsSuccess);
    }
    
    [Fact]
    public void ShouldFail_WhenInventoryCountIsNegative()
    {
        // Arrange
        var inventoryCount = InventoryCount.Create(-1);
        Assert.False(inventoryCount.IsSuccess);
    }
    
    [Fact]
    public void ShouldFail_WhenPriceIsNegative()
    {
        // Arrange
        var price = Price.Create(-1);
        Assert.False(price.IsSuccess);
    }
}