using Domain.Common.BaseClasses;
using Domain.Common.OperationResult;

namespace Domain.Aggregates.Service.Values;

public class Description : ValueObject
{
    
    internal string Value { get; private set; }
    
    private Description(string value)
    {
        Value = value;
    }
    
    public static Result<Description> Create(string description)
    {
        
        if (string.IsNullOrWhiteSpace(description))
        {
            return Result<Description>.Fail(ServiceErrorMessage.EmptyServiceDescription());
        }
        
        return Result<Description>.Success(new Description(description));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}