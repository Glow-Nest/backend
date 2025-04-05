using Application.Common;
using Application.Interfaces;
using Domain.Aggregates.Client;
using Domain.Common.OperationResult;

namespace UnitTest.Features.Helpers;

public class FakeEmailSender : IEmailSender
{
    
    public async Task<Result> SendEmailAsync(Client email, EmailPurpose purpose, string subject, string message)
    {
        // Simulate sending an email
        return await Task.FromResult(Result.Success());
    }
}