using Domain.Aggregates.Client.Values;
using Domain.Aggregates.SalonOwner;
using Domain.Common.OperationResult;

namespace DomainModelPersistence.SalonOwnerPersistence;

public class SalonOwnerRepository: ISalonOwnerRepository
{
    private readonly List<SalonOwner> _listOfSalonOwners = new();
    
    //This is just for test purpose
    private readonly string _salonOwnerEmail = "salonowner@gmail.com";
    
    public async Task<Result> AddAsync(SalonOwner salonOwner)
    {
        if (salonOwner.EmailValue.Equals(Email.Create(_salonOwnerEmail).Data))
        {
            _listOfSalonOwners.Add(salonOwner);
            return await Task.FromResult(Result.Success());
        }
        return await Task.FromResult(Result.Fail(SalonOwnerErrorMessage.SalonOwnerNotFound()));
    }

    public async Task<Result<SalonOwner>> GetAsync(Email email)
    {
        // Check if the email matches the hardcoded salon owner email
        if (email.Equals(Email.Create(_salonOwnerEmail).Data))
        {
            var salonOwner = _listOfSalonOwners.FirstOrDefault(c => c.EmailValue.Equals(email));
            if (salonOwner == null)
            {
                return await Task.FromResult(Result<SalonOwner>.Fail(SalonOwnerErrorMessage.SalonOwnerNotFound()));
            }
            return await Task.FromResult(Result<SalonOwner>.Success(salonOwner));
        }

        return await Task.FromResult(Result<SalonOwner>.Fail(SalonOwnerErrorMessage.SalonOwnerNotFound()));
    }
}