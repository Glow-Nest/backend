using OperationResult;

namespace Domain.Common;

public class GenericErrorMessage
{
    public static Error MissingConfiguration() =>new Error("Token.IsMissingConfiguration", "The secret key is not configured.");
    public static Error KeyIsTooShort () =>new Error("Token.IsShort", "The secret key is too short. Must be at least 128 bits.");
    public static Error EntityNotFound() => new Error("Entity.NotFound", "Entity not found in the database");
    public static Error ErrorInSaving()=> new Error("Error in saving changes", "An error occurred while saving changes to the database.");
    
    public static Error ErrorParsingTime() => new Error("Error in parsing time", "An error occurred while parsing the time.");
    public static Error ErrorParsingPrice() => new Error("Error in parsing price", "An error occurred while parsing the price.");
    public static Error ErrorParsingDate() => new Error("Error in parsing date", "An error occurred while parsing the date.");
    public static Error ErrorParsingGuid() => new Error("Error in parsing guid", "An error occurred while parsing the guid.");
    
    public static Error TimeMustBeOnHalfHour() => new Error("Error in parsing time", "Start and end times must be on the hour or half-hour (e.g., 10:00, 10:30).");
}