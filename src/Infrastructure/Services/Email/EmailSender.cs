using Application.Common;
using Application.Interfaces;
using Domain.Aggregates.Client;
using Domain.Common.OperationResult;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Services.Utilities;

namespace Services.Email;

public class EmailSender : IEmailSender
{
    private readonly string smtpServer;
    private readonly int smtpPort;
    private readonly string fromEmail;
    private readonly string fromName;
    private readonly string appPassword;

    public EmailSender(IConfiguration config)
    {
        smtpServer = config["Smtp:Server"]!;
        smtpPort = int.Parse(config["Smtp:Port"]!);
        fromEmail = config["Smtp:FromEmail"]!;
        fromName = config["Smtp:FromName"]!;
        appPassword = config["Smtp:AppPassword"]!;
    }

    public async Task<Result> SendEmailAsync(Client to, EmailPurpose purpose, string subject, string body)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(fromName, fromEmail));
            message.To.Add(new MailboxAddress(to.FullName.ToString(), to.EmailAddress));
            message.Subject = subject;

            string finalBody = BuildEmailBody(purpose, to.FullName.ToString(), subject, body);

            message.Body = new TextPart("plain")
            {
                Text = finalBody
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(fromEmail, appPassword);
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
