using Domain.Aggregates.Client;
using Domain.Aggregates.Client.Values;
using UnitTest.Features.Helpers;

namespace UnitTest.Features.ClientTest.UpdatePhoneNumberTest;

public class UpdatePhoneNumberAggregateTest
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

        var client = await Client.Create(fullName, email, password, phoneNumber, emailUniqueChecker);
        var updatePhoneNumber = PhoneNumber.Create("22222222").Data;

        // Act
        var result = client.Data.UpdatePhoneNumber(updatePhoneNumber);

        // Assert
        Assert.True(result.IsSuccess);
    }
}