using Application.Common;
using Application.Interfaces;
using Domain.Aggregates.Client;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using OperationResult;

namespace Services.Email;

public class EmailSender : IEmailSender
{
    private readonly EmailSettings _settings;

    public EmailSender(IOptions<EmailSettings> options)
    {
        _settings = options.Value;
    }

    public async Task<Result> SendEmailAsync(Client to, EmailPurpose purpose, string subject, string body)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));
            message.To.Add(new MailboxAddress(to.FullName.ToString(), to.EmailAddress));
            message.Subject = subject;

            string finalBody = BuildEmailBody(purpose, to.FullName.ToString(), subject, body);

            message.Body = new TextPart("plain")
            {
                Text = finalBody
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(_settings.SmtpServer, _settings.SmtpPort, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_settings.FromEmail, _settings.AppPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Fail(new Error("Error.SendingEmail", e.Message));
        }
    }

    private string BuildEmailBody(EmailPurpose purpose, string recipientName, string subject, string body)
    {
        return purpose switch
        {
            EmailPurpose.OtpCode => EmailMessageBuilder.BuildOtpMessage(recipientName, body, TimeSpan.FromMinutes(2)),
            _ => EmailMessageBuilder.BuildGenericMessage(recipientName, subject, body)
        };
    }
}