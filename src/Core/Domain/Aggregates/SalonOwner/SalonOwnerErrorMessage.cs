using Domain.Common.OperationResult;

namespace Domain.Aggregates;

public class SalonOwnerErrorMessage
{
    public static Error SalonOwnerNotFound() => new("SalonOwner.NotFound", "SalonOwner not found.");
}