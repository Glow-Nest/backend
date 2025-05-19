using Domain.Aggregates.Client.Values;
using Domain.Aggregates.Order;
using Domain.Aggregates.Order.Contracts;
using Domain.Aggregates.Order.Values;
using Domain.Aggregates.Product.Values;
using Domain.Common.Contracts;
using Domain.Common.Values;
using Moq;

namespace UnitTest.Features.OrderTest;

public class MarkAsPaidAggregateTest
{
    [Fact]
    public async void MarkAsPaid_WhenOrderStatusIsCreated_ShouldSetStatusToPaid()
    {
        // Arrange
        var order = await CreateOrder();

        // Act
        var result = order.MarkAsPaid();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(OrderStatus.Paid, order.OrderStatus);
    }

    [Fact]
    public async void MarkAsPaid_WhenOrderStatusIsNotCreated_ShouldFail()
    {
        // Arrange
        var order = await CreateOrder();

        order.MarkAsPaid();
        order.MarkAsReadyForPickup();

        // Act
        var result = order.MarkAsPaid();

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(OrderStatus.ReadyForPickup, order.OrderStatus);
    }

    private async Task<Order> CreateOrder()
    {
        Mock<IProductChecker> productCheckerMock = new Mock<IProductChecker>();
        Mock<IDateTimeProvider> dateTimeProviderMock = new Mock<IDateTimeProvider>();
        
        productCheckerMock.Setup(x => x.DoesProductExist(It.IsAny<ProductId>()))
            .ReturnsAsync(true);
        dateTimeProviderMock.Setup(x => x.GetNow()).Returns(DateTime.Now);
        
        var clientId = ClientId.FromGuid(Guid.NewGuid());
        var pickupDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1));
        var productId = ProductId.FromGuid(Guid.NewGuid());
        var quantity = Quantity.Create(1).Data;
        var price = Price.Create(100).Data;
        var orderItem = new OrderItemDto(productId, quantity, price);
        var order = await Order.Create(clientId, [orderItem], pickupDate, dateTimeProviderMock.Object, productCheckerMock.Object);

        return order.Data;
    }
}