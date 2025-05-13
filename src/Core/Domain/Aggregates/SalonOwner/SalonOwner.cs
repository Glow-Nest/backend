using Domain.Aggregates.Client.Values;
using Domain.Aggregates.SalonOwner.Values;
using Domain.Common.BaseClasses;
using OperationResult;

namespace Domain.Aggregates.SalonOwner;

public class SalonOwner : AggregateRoot
{
    public SalonOwnerId SalonOwnerId { get; private set; }
    internal Email Email { get; private set; }
    internal Password Password { get; private set; }

    public SalonOwner() // For EF
    {
    }

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