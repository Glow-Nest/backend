using Application.AppEntry.Commands.Client;
using Application.Handlers.ClientHandlers;
using Application.Interfaces;
using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Contracts;
using Domain.Aggregates.Client.Values;
using Domain.Common.OperationResult;
using Moq;
using UnitTest.Features.Helpers;
using UnitTest.Features.Helpers.Builders;

namespace UnitTest.Features.ClientTest.CreateClient.CreateOtp;

public class CreateOtpHandlerTest
{
    private readonly Mock<IDateTimeProvider> _mockDateTimeProvider = new();
    private readonly IEmailSender _emailSender= new FakeEmailSender();


    [Fact]
    public async void ShouldSucceed_WhenValidCommandProvided()
    {
        // Arrange
        var repository = new FakeClientRepository();
        var fakeUnitOfWork = new FakeUnitOfWork();

        var now = DateTime.UtcNow;
        _mockDateTimeProvider.Setup(d => d.GetNow()).Returns(now.AddMinutes(5));

        var client = await ClientBuilder.CreateValid().BuildAsync();
        await repository.AddAsync(client.Data);



        var otpCommand = CreateOtpCommand.Create("validU@gmail.com", "Registration").Data;
        var createOtpHandler = new CreateOtpHandler(repository, _mockDateTimeProvider.Object, fakeUnitOfWork, _emailSender);

        // Act
        var result = await createOtpHandler.HandleAsync(otpCommand);

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async void ShouldFail_WhenInvalidEmailProvided()
    {
        // Arrange
        var repository = new FakeClientRepository();
        var fakeUnitOfWork = new FakeUnitOfWork();

        var now = DateTime.UtcNow;
        _mockDateTimeProvider.Setup(d => d.GetNow()).Returns(now.AddMinutes(5));

        var otpCommand = CreateOtpCommand.Create("invalidEmail@gmail.com", "Registration").Data;
        var createOtpHandler = new CreateOtpHandler(repository, _mockDateTimeProvider.Object, fakeUnitOfWork, _emailSender);

        // Act
        var result = await createOtpHandler.HandleAsync(otpCommand);

        // Assert
        Assert.False(result.IsSuccess);
    }
}