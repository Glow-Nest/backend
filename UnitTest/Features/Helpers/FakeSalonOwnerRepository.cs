using Domain.Aggregates.Client.Values;
using Domain.Aggregates.SalonOwner;
using Domain.Aggregates.SalonOwner.Values;
using Domain.Common.OperationResult;
using Domain.Common.Repositories;

namespace UnitTest.Features.Helpers;

public class FakeSalonOwnerRepository : ISalonOwnerRepository
{
    private List<SalonOwner> _listOfSalonOwners = new();
    
    Task<Result> ISalonOwnerRepository.AddAsync(SalonOwner salonOwner)
    {
        _listOfSalonOwners.Add(salonOwner);
        return Task.FromResult(Result.Success());
    }

    public async Task<Result> AddAsync(SalonOwner salonOwner)
    {
        _listOfSalonOwners.Add(salonOwner);
        return await Task.FromResult(Result.Success());
    }
    Task<Result<SalonOwner>> ISalonOwnerRepository.GetAsync(SalonOwnerId salonOwnerId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<SalonOwner>> GetAsync(Email email)
    {
        var salonOwner = _listOfSalonOwners.FirstOrDefault(c => c.Email.Equals(email));
        if (salonOwner == null)
        {
            return await Task.FromResult(Result<SalonOwner>.Fail(SalonOwnerErrorMessage.SalonOwnerNotFound()));
        }

        return await Task.FromResult(Result<SalonOwner>.Success(salonOwner));
    }

    Task<Result<SalonOwner>> IGenericRepository<SalonOwner, SalonOwnerId>.GetAsync(SalonOwnerId id)
    {
        throw new NotImplementedException();
    }

    public Task<Result> RemoveAsync(SalonOwnerId id)
    {
        throw new NotImplementedException();
    }

    Task<Result> IGenericRepository<SalonOwner, SalonOwnerId>.AddAsync(SalonOwner aggregate)
    {
        throw new NotImplementedException();
    }
}