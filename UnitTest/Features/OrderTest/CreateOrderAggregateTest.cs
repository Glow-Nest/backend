using Domain.Aggregates.Client.Values;
using Domain.Aggregates.Order;
using Domain.Aggregates.Order.Contracts;
using Domain.Aggregates.Order.Values;
using Domain.Aggregates.Product;
using Domain.Aggregates.Product.Values;
using Domain.Common.Contracts;
using Domain.Common.Values;
using Moq;

namespace UnitTest.Features.OrderTest;

public class OrderTests
{
    private readonly Mock<IDateTimeProvider> dateTimeProviderMock = new();

    [Fact]
    public async Task CreateOrder_ValidInput_ReturnsSuccess()
    {
        // Arrange
        var clientId = ClientId.Create();
        var productId = ProductId.Create();
        var pickupDate = DateOnly.FromDateTime(DateTime.Now).AddDays(3);
        var expectedOrderDate = DateOnly.FromDateTime(DateTime.Now).AddDays(2);
        
        // mock product checker
        var productChecker = new Mock<IProductChecker>();
        productChecker.Setup(x => x.DoesProductExist(productId)).ReturnsAsync(true);
        
        var orderItemDto = new OrderItemDto(productId, Quantity.Create(4).Data, Price.Create(50).Data);

        dateTimeProviderMock.Setup(d => d.GetNow()).Returns(DateTime.Now.AddDays(2));

        // Act
        var result = await Order.Create(clientId, [orderItemDto], pickupDate, dateTimeProviderMock.Object, productChecker.Object);

        // Assert
        Assert.True(result.IsSuccess);
        var order = result.Data;
        Assert.Equal(clientId, order.ClientId);
        Assert.Equal(expectedOrderDate, order.OrderDate);
        Assert.Single(order.OrderItems);
        Assert.Equal(200, order.TotalPrice.Value);
        Assert.Equal(OrderStatus.Created, order.OrderStatus);
        Assert.Equal(PaymentStatus.Pending, order.PaymentStatus);
    }
    
    [Fact]
    public async Task CreateOrder_WithNonExistingProduct_ReturnsFailure()
    {
        // Arrange
        var clientId = ClientId.Create();
        var productId = ProductId.Create();
        var pickupDate = DateOnly.FromDateTime(DateTime.Now).AddDays(3);
    
        var productChecker = new Mock<IProductChecker>();
        productChecker.Setup(x => x.DoesProductExist(productId)).ReturnsAsync(false);

        var orderItemDto = new OrderItemDto(productId, Quantity.Create(1).Data, Price.Create(50).Data);

        dateTimeProviderMock.Setup(d => d.GetNow()).Returns(DateTime.Now.AddDays(2));

        // Act
        var result = await Order.Create(clientId, [orderItemDto], pickupDate, dateTimeProviderMock.Object, productChecker.Object);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e.Message == ProductErrorMessage.ProductNotFound().Message);
    }
    
    [Fact]
    public async Task CreateOrder_WithEmptyOrderItems_ReturnsFailure()
    {
        // Arrange
        var clientId = ClientId.Create();
        var pickupDate = DateOnly.FromDateTime(DateTime.Now).AddDays(1);
    
        var productChecker = new Mock<IProductChecker>();
        var orderItems = new List<OrderItemDto>();
        
        dateTimeProviderMock.Setup(d => d.GetNow()).Returns(DateTime.Now.AddDays(2));

        // Act
        var result = await Order.Create(clientId, orderItems, pickupDate, dateTimeProviderMock.Object, productChecker.Object);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e.Message == OrderErrorMessage.NoOrderItems().Message);
    }



}