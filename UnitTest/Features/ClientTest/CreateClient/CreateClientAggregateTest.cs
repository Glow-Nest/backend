using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Contracts;
using Domain.Aggregates.Client.Entities;
using Domain.Aggregates.Client.Values;
using Moq;
using UnitTest.Features.Helpers;
using UnitTest.Features.Helpers.Builders;

namespace UnitTest.Features.ClientTest.CreateClient;

public class CreateClientAggregateTest
{

    [Fact]
    public async void ShouldSucceed_WhenValidClientProvided()
    {
        // Arrange
        var fullName = FullName.Create("Valid", "User").Data;
        var email = Email.Create("validU@gmail.com").Data;
        var password = Password.Create("Password@123").Data;
        var phoneNumber = PhoneNumber.Create("91000000").Data;

        var clientRepository = new FakeClientRepository();
        var emailUniqueChecker = new FakeEmailUniqueChecker(clientRepository);

        // Act
        var result = await Client.Create(fullName, email, password, phoneNumber, emailUniqueChecker);

        if (result.IsSuccess)
        {
            await clientRepository.AddAsync(result.Data);
        }

        // Assert
        Assert.True(result.IsSuccess);
    }

    [Fact]
    public async void ShouldFail_WhenEmailIsNotUnique()
    {
        // Arrange
        var clientRepository = new FakeClientRepository();
        var emailUniqueChecker = new FakeEmailUniqueChecker(clientRepository);

        var firstClient = await ClientBuilder.CreateValid().BuildAsync();
        await clientRepository.AddAsync(firstClient.Data);

        var fullName = FullName.Create("Valid", "User").Data;
        var email = Email.Create("validU@gmail.com").Data;
        var password = Password.Create("Password@123").Data;
        var phoneNumber = PhoneNumber.Create("91000000").Data;

        // Act
        var result = await Client.Create(fullName, email, password, phoneNumber, emailUniqueChecker);

        if (result.IsSuccess)
        {
            await clientRepository.AddAsync(result.Data);
        }

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Contains(ClientErrorMessage.EmailAlreadyExists(), result.Errors);
    }
}