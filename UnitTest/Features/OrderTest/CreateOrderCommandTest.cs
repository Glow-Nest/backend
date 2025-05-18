using System.Globalization;
using Application.AppEntry.Commands.Order;

namespace UnitTest.Features.OrderTest;

public class CreateOrderCommandTests
{
    [Fact]
    public void Create_ValidInput_ReturnsSuccess()
    {
        // Arrange
        var clientId = Guid.NewGuid().ToString();
        var pickupDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        var totalPrice = 200.0;

        var orderItemDtos = new List<OrderItemDto>
        {
            new(Guid.NewGuid().ToString(), 2, 100.0)
        };

        // Act
        var result = CreateOrderCommand.Create(clientId, totalPrice, pickupDate, orderItemDtos);

        // Assert
        Assert.True(result.IsSuccess);
        var command = result.Data;
        Assert.Equal(totalPrice, command.TotalPrice.Value);
        Assert.Single(command.OrderItems);
    }

    [Theory]
    [InlineData("invalid-guid", "2025-05-15", 100.0)] // invalid clientId
    [InlineData("00000000-0000-0000-0000-000000000000", "invalid-date", 100.0)] // invalid pickup date
    [InlineData("00000000-0000-0000-0000-000000000000", "2025-05-15", -100.0)] // invalid price
    public void Create_InvalidInput_ReturnsFailure(string clientId, string pickupDate, double totalPrice)
    {
        // Arrange
        var orderItemDtos = new List<OrderItemDto>
        {
            new(Guid.NewGuid().ToString(), 1, 100.0)
        };

        // Act
        var result = CreateOrderCommand.Create(clientId, totalPrice, pickupDate, orderItemDtos);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Errors);
    }

    [Fact]
    public void Create_WithInvalidOrderItemDto_ReturnsFailure()
    {
        // Arrange
        var clientId = Guid.NewGuid().ToString();
        var pickupDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        var totalPrice = 100.0;

        var orderItemDtos = new List<OrderItemDto>
        {
            new("invalid-guid", -5, -99.0)
        };

        // Act
        var result = CreateOrderCommand.Create(clientId, totalPrice, pickupDate, orderItemDtos);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.NotEmpty(result.Errors);
    }
}