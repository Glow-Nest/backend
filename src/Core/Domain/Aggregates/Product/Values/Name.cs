using Domain.Common.BaseClasses;
using OperationResult;

namespace Domain.Aggregates.Product.Values;

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
            return Result<Name>.Fail(ProductErrorMessage.EmptyProductName());
        }
        
        return Result<Name>.Success(new Name(name));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}