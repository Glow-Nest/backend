﻿using Application.AppEntry.Commands.Client;
using Application.Handlers.ClientHandlers;
using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Values;
using Domain.Common.Contracts;
using Moq;
using OperationResult;
using UnitTest.Features.Helpers.Builders;

namespace UnitTest.Features.ClientTest.PasswordTest.InitiateResetPassword;

public class InitiateResetPasswordHandlerTest
{
    private readonly Mock<IClientRepository> _clientRepositoryMock;
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
    private readonly InitiateResetPasswordHandler _handler;
    
    public InitiateResetPasswordHandlerTest()
    {
        _clientRepositoryMock = new Mock<IClientRepository>();
        _dateTimeProviderMock = new Mock<IDateTimeProvider>();
        _dateTimeProviderMock.Setup(p => p.GetNow()).Returns(DateTime.UtcNow);
        _handler = new InitiateResetPasswordHandler(_clientRepositoryMock.Object, _dateTimeProviderMock.Object);
    }
    
    [Fact]
    public async Task ShouldReturnSuccess_WhenOtpIsSuccessfullyGenerated()
    {
        // Arrange  
        var clientResult = await ClientBuilder.CreateValid().BuildAsync();
        var client = clientResult.Data;

        var email = client.Email.Value;
        var command = InitiateResetPasswordCommand.Create(email);

        _clientRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Email>()))
            .ReturnsAsync(Result<Client>.Success(client));

        // Act
        var result = await _handler.HandleAsync(command.Data);

        // Assert
        Assert.True(result.IsSuccess);
    }
    
    [Fact]
    public async Task ShouldReturnFailure_WhenClientNotFound()
    {
        // Arrange
        var command = InitiateResetPasswordCommand.Create("nonexistent@example.com");

        _clientRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Email>()))
            .ReturnsAsync(Result<Client>.Fail(ClientErrorMessage.ClientNotFound()));

        // Act
        var result = await _handler.HandleAsync(command.Data);

        // Assert
        Assert.False(result.IsSuccess);
    }
}