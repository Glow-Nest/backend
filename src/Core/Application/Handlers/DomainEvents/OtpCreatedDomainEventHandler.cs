using Application.AppEntry;
using Application.Common;
using Application.Interfaces;
using Domain.Aggregates.Client.DomainEvents;
using Domain.Common.OperationResult;

namespace Application.Handlers.DomainEvents;

public class OtpCreatedDomainEventHandler :IDomainEventHandler<OtpCreatedDomainEvent>
{
    private readonly IEmailSender _emailSender;

    public OtpCreatedDomainEventHandler(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    public async Task<Result> HandleAsync(OtpCreatedDomainEvent domainEvent)
    {
        var sendEmailResult = await _emailSender.SendEmailAsync(domainEvent.Client, EmailPurpose.OtpCode, "OTP Code", domainEvent.OtpCode.ToString());
        
        if (!sendEmailResult.IsSuccess)
        {
            return sendEmailResult;
        }
        
            return Result.Success();
    }
}