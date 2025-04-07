using Domain.Common.OperationResult;

namespace Domain.Common;

public class GenericErrorMessage
{
    public static Error MissingConfiguration() =>new Error("Token.IsMissingConfiguration", "The secret key is not configured.");
    public static Error KeyIsTooShort () =>new Error("Token.IsShort", "The secret key is too short. Must be at least 128 bits.");
    public static Error EntityNotFound() => new Error("Entity.NotFound", "Entity not found in the database");
    public static Error ErrorInSaving()=> new Error("Error in saving changes", "An error occurred while saving changes to the database.");
}