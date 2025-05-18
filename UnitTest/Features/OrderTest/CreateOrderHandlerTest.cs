using Application.AppEntry.Commands.Order;
using Application.Handlers.OrderHandlers;
using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Values;
using Domain.Aggregates.Order;
using Domain.Aggregates.Order.Contracts;
using Domain.Aggregates.Order.Entities;
using Domain.Aggregates.Order.Values;
using Domain.Aggregates.Product.Values;
using Domain.Common.Contracts;
using Domain.Common.Values;
using Moq;
using OperationResult;
using UnitTest.Features.Helpers.Builders;

namespace UnitTest.Features.OrderTest;

public class CreateOrderHandlerTests
{
    private readonly Mock<IOrderRepository> _orderRepoMock = new();
    private readonly Mock<IClientRepository> _clientRepoMock = new();
    private readonly Mock<IDateTimeProvider> _dateTimeMock = new();
    private readonly Mock<IProductChecker> _productCheckerMock = new();

    private readonly ClientId _clientId = ClientId.Create();
    private readonly ProductId _productId = ProductId.Create();
    private readonly Price _price = Price.Create(100).Data;
    private readonly Quantity _quantity = Quantity.Create(2).Data;

    private CreateOrderCommand CreateValidCommand()
    {
        var orderItem = OrderItem.Create(_productId, _quantity, _price).Data;
        var totalPrice = Price.Create(_price.Value * _quantity.Value).Data;

        return new CreateOrderCommand(totalPrice, _clientId, [orderItem]);
    }

    [Fact]
    public async Task HandleAsync_ValidOrder_ReturnsSuccess()
    {
        // Arrange
        var clientResult = await ClientBuilder.CreateValid().BuildAsync();

        var command = CreateValidCommand();
        
        _productCheckerMock.Setup(p => p.DoesProductExist(_productId))
            .ReturnsAsync(true);
        
        var orderResult = Order.Create(_clientId, command.OrderItems, _dateTimeMock.Object, _productCheckerMock.Object).Result;
        var order = orderResult.Data;
        

        _clientRepoMock.Setup(repo => repo.GetAsync(_clientId))
                       .ReturnsAsync(Result<Client>.Success(clientResult.Data));

        _orderRepoMock.Setup(repo => repo.AddAsync(It.IsAny<Order>()))
                      .ReturnsAsync(Result.Success());
        
        var handler = new CreateOrderHandler(_orderRepoMock.Object, _clientRepoMock.Object, _dateTimeMock.Object, _productCheckerMock.Object);

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async Task HandleAsync_ClientNotFound_ReturnsFailure()
    {
        // Arrange
        var command = CreateValidCommand();

        _clientRepoMock.Setup(repo => repo.GetAsync(_clientId))
                       .ReturnsAsync(Result<Client>.Fail([ClientErrorMessage.ClientNotFound()]));

        var handler = new CreateOrderHandler(_orderRepoMock.Object, _clientRepoMock.Object, _dateTimeMock.Object, _productCheckerMock.Object);

        // Act
        var result = await handler.HandleAsync(command);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(result.Errors, e => e.Message == ClientErrorMessage.ClientNotFound().Message);
    }

    // [Fact]
    // public async Task HandleAsync_OrderCreationFails_ReturnsFailure()
    // {
    //     // Arrange
    //     var command = CreateValidCommand();
    //
    //     _clientRepoMock.Setup(repo => repo.GetAsync(_clientId))
    //                    .ReturnsAsync(Result<Client>.Success(new Client()));
    //
    //     _productCheckerMock.Setup(p => p.DoesProductExist(It.IsAny<ProductId>()))
    //                        .ReturnsAsync(true);
    //
    //     // Simulate bad pickup date
    //     _dateTimeMock.Setup(d => d.GetNow()).Returns(DateTime.Today.AddDays(5));
    //
    //     var handler = new CreateOrderHandler(_orderRepoMock.Object, _clientRepoMock.Object, _dateTimeMock.Object, _productCheckerMock.Object);
    //
    //     // Act
    //     var result = await handler.HandleAsync(command);
    //
    //     // Assert
    //     Assert.False(result.IsSuccess);
    //     Assert.Contains(result.Errors, e => e.Message == OrderErrorMessage.PickupDateInThePast().Message);
    // }
}
