using Domain.Common.BaseClasses;
using Domain.Common.OperationResult;

namespace Domain.Aggregates.Service.Values;

public class Name : ValueObject
{
    
    internal string Value { get; private set; }
    
    private Name(string value)
    {
        Value = value;
    }

    public static Result<Name> Create(string name)
    {
        
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result<Name>.Fail(ServiceErrorMessage.EmptyServiceName());
        }
        
        return Result<Name>.Success(new Name(name));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}