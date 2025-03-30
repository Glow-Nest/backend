using Domain.Aggregates.Client.Values;
using Domain.Aggregates.Values;
using Domain.Common.BaseClasses;
using Domain.Common.OperationResult;

namespace Domain.Aggregates;

public class SalonOwner : AggregateRoot<SalonOwnerId>
{
    internal SalonOwnerId SalonOwnerId { get; private set; }
    internal Email Email { get; private set; }
    internal Password Password { get; private set; }
    
    public string EmailAddress => Email.Value;
    public Email EmailValue => Email;
    public Password PasswordValue => Password;
    private SalonOwner(SalonOwnerId salonOwnerId, Email email, Password password)
    {
        SalonOwnerId = salonOwnerId;
        Email = email;
        Password = password;
    }
    
    public static Result<SalonOwner> Create(Email email, Password password)
    {
        var Id = SalonOwnerId.Create();

        var salonOwner = new SalonOwner(Id, email, password);
        return Result<SalonOwner>.Success(salonOwner);
    }
}