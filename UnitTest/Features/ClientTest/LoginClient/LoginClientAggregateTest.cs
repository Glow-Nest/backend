using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Contracts;
using Domain.Aggregates.Client.Values;
using Domain.Common.OperationResult;
using DomainModelPersistence.ClientPersistence;
using DomainModelPersistence.EfcConfigs;
using Moq;
using Services.Authentication;
using UnitTest.Features.Helpers;

namespace UnitTest.Features.ClientTest.LoginClient;

public class LoginClientAggregateTest
{

    [Fact]
    public async Task ShouldSucceed_WhenEmailExists()
    {
        // Arrange
        var email = Email.Create("validClient@example.com").Data;
        var password = Password.Create("Password123!").Data;
        var fullname = FullName.Create("Suhani", "Pandey").Data;
        var phoneNumber = PhoneNumber.Create("91000000").Data;
        
        var clientRepository = new FakeClientRepository();
        var emailUniqueChecker = new FakeEmailUniqueChecker(clientRepository);
        
        var client=Client.Create(fullname, email, password, phoneNumber, emailUniqueChecker);
        await clientRepository.AddAsync(client.Result.Data);
        
        // Act
        var result = await clientRepository.GetAsync(email);
        
        // Assert
        Assert.True(result.IsSuccess);
    }
    
    [Fact]
    public async Task ShouldSucceed_WhenEmailExists_AndPasswordIsCorrect()
    {
        // Arrange
        var email = Email.Create("validClient@example.com").Data;
        var password = Password.Create("Password123!").Data;
        var fullname = FullName.Create("Suhani", "Pandey").Data;
        var phoneNumber = PhoneNumber.Create("91000000").Data;

        var clientRepository = new FakeClientRepository();
        var emailUniqueChecker = new FakeEmailUniqueChecker(clientRepository);
        
        var clientResult = Client.Create(fullname, email, password, phoneNumber, emailUniqueChecker);
        await clientRepository.AddAsync(clientResult.Result.Data);
        
        // Act
        var result = await clientRepository.GetAsync(email);
        
        // Assert
        Assert.True(result.IsSuccess);
        
        // Verify if password is correct
        var client = result.Data;
        Assert.True(client.Password.Verify("Password123!"));
    }
}