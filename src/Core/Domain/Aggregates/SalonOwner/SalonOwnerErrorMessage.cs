using OperationResult;

namespace Domain.Aggregates.SalonOwner;

public class SalonOwnerErrorMessage
{
    public static Error SalonOwnerNotFound() => new("SalonOwner.NotFound", "SalonOwner not found.");
}