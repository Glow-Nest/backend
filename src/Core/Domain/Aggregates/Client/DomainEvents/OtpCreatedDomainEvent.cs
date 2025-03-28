using Domain.Aggregates.Client.Values;
using Domain.Common;

namespace Domain.Aggregates.Client.DomainEvents;

public class OtpCreatedDomainEvent : IDomainEvent
{
    public OtpCode OtpCode { get; private set; }
    public Client Client { get; private set; }

    public OtpCreatedDomainEvent(OtpCode otpCode, Client client)
    {
        OtpCode = otpCode;
        Client = client;
    }
}