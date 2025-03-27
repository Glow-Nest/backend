using Application.Common;
using Domain.Aggregates.Client;
using Domain.Common.OperationResult;

namespace Application.Interfaces;

public interface IEmailSender
{
    Task<Result> SendEmailAsync(Client email, EmailPurpose purpose, string subject, string message);
}
