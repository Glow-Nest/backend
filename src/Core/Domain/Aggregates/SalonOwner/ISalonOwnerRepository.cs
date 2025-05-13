using Domain.Aggregates.Client.Values;
using Domain.Aggregates.SalonOwner.Values;
using Domain.Common.Repositories;
using OperationResult;

namespace Domain.Aggregates.SalonOwner;

public interface ISalonOwnerRepository : IGenericRepository<SalonOwner, SalonOwnerId>
{
    Task<Result> AddAsync(SalonOwner salonOwner);
    Task<Result<SalonOwner>> GetAsync(SalonOwnerId salonOwnerId);
    Task<Result<SalonOwner>> GetAsync(Email email);
}