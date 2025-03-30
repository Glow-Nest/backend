using Domain.Aggregates.Client.Values;
using Domain.Common.OperationResult;

namespace Domain.Aggregates;

public interface ISalonOwnerRepository
{
    Task<Result> AddAsync(SalonOwner salonOwner);
    Task<Result<SalonOwner>> GetAsync(Email email);
}