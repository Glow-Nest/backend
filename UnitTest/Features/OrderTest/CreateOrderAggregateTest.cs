using Domain.Aggregates.Client.Values;
using Domain.Aggregates.Order;
using Domain.Aggregates.Order.Contracts;
using Domain.Aggregates.Order.Entities;
using Domain.Aggregates.Order.Values;
using Domain.Aggregates.Product;
using Domain.Aggregates.Product.Values;
using Domain.Common.Contracts;
using Domain.Common.Values;
using Moq;

namespace UnitTest.Features.OrderTest;

public class OrderTests
{
    private class FakeDateTimeProvider : IDateTimeProvider
    {
        public DateTime GetNow() => new DateTime(2025, 05, 10);
    }

    [Fact]
    public async void CreateOrder_ValidInput_ReturnsSuccess()
    {
        // Arrange
        var clientId = ClientId.Create();
        var productId = ProductId.Create();
        
        // mock product checker
        var productChecker = new Mock<IProductChecker>();
        productChecker.Setup(x => x.DoesProductExist(productId)).ReturnsAsync(true);

        var orderItems = new List<OrderItem>
        {
            OrderItem.Create(productId, Quantity.Create(2).Data, Price.Create(100).Data).Data
        };

        var dateTimeProvider = new FakeDateTimeProvider();

        // Act
        var result = await Order.Create(clientId, orderItems, dateTimeProvider, productChecker.Object);

        // Assert
        Assert.True(result.IsSuccess);
        var order = result.Data;
        Assert.Equal(clientId, order.ClientId);
        Assert.Equal(new DateOnly(2025, 05, 10), order.OrderDate);
        Assert.Single(order.OrderItems);
        Assert.Equal(200, order.TotalPrice.Value);
        Assert.Equal(OrderStatus.Created, order.OrderStatus);
        Assert.Equal(PaymentStatus.Pending, order.PaymentStatus);
    }

    [Fact]
    public async void CreateOrder_PickupDateInThePast_ReturnsFailure()
    {
        // Arrange
        var clientId = ClientId.Create();
        var productId = ProductId.Create();
        
        // mock product checker
        var productChecker = new Mock<IProductChecker>();
        productChecker.Setup(x => x.DoesProductExist(productId)).ReturnsAsync(true);

        var orderItems = new List<OrderItem>
        {
            OrderItem.Create(productId, Quantity.Create(1).Data, Price.Create(50).Data).Data
        };

        var dateTimeProvider = new FakeDateTimeProvider();

        // Act
        var result = await Order.Create(clientId, orderItems, dateTimeProvider, productChecker.Object);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e.Message == OrderErrorMessage.PickupDateInThePast().Message);
    }
    
    [Fact]
    public async void CreateOrder_WithNonExistingProduct_ReturnsFailure()
    {
        // Arrange
        var clientId = ClientId.Create();
        var productId = ProductId.Create();
    
        var productChecker = new Mock<IProductChecker>();
        productChecker.Setup(x => x.DoesProductExist(productId)).ReturnsAsync(false);

        var orderItems = new List<OrderItem>
        {
            OrderItem.Create(productId, Quantity.Create(1).Data, Price.Create(50).Data).Data
        };

        var dateTimeProvider = new FakeDateTimeProvider();

        // Act
        var result = await Order.Create(clientId, orderItems, dateTimeProvider, productChecker.Object);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e.Message == ProductErrorMessage.ProductNotFound().Message);
    }
    
    [Fact]
    public async void CreateOrder_WithEmptyOrderItems_ReturnsFailure()
    {
        // Arrange
        var clientId = ClientId.Create();
    
        var productChecker = new Mock<IProductChecker>();

        var orderItems = new List<OrderItem>();

        var dateTimeProvider = new FakeDateTimeProvider();

        // Act
        var result = await Order.Create(clientId, orderItems, dateTimeProvider, productChecker.Object);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e.Message == OrderErrorMessage.NoOrderItems().Message);
    }



}