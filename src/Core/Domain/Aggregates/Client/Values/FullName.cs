using Domain.Common.BaseClasses;
using Domain.Common.OperationResult;

namespace Domain.Aggregates.Client.Values;

public class FullName : ValueObject
{
    internal string FirstName { get; }
    internal string LastName { get; }
    
    private FullName(string firstName, string lastName)
    {
        FirstName = char.ToUpper(firstName[0]) + firstName.Substring(1).ToLower();
        LastName = char.ToUpper(lastName[0]) + lastName.Substring(1).ToLower();
    }
    
    public static Result<FullName> Create(string firstName, string lastName)
    {   
        if (string.IsNullOrWhiteSpace(firstName) || firstName.Length < 2 || firstName.Length > 25 || !firstName.All(char.IsLetter))
        {
            return Result<FullName>.Fail(ClientErrorMessage.InvalidFirstName());
        }
        if (string.IsNullOrWhiteSpace(lastName) || lastName.Length < 2 || lastName.Length > 25 || !lastName.All(char.IsLetter))
        {
            return Result<FullName>.Fail(ClientErrorMessage.InvalidLastName());
        }
        return Result<FullName>.Success(new FullName(firstName, lastName));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstName + LastName;
    }
}