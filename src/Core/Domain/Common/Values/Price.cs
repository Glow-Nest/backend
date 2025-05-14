using Domain.Aggregates.Product;
using Domain.Common.BaseClasses;
using OperationResult;

namespace Domain.Common.Values;

public class Price : ValueObject
{
    internal double Value { get; private set; }
    
    private Price(double value)
    {
        Value = value;
    }
    
    public static Result<Price> Create(double price)
    {
        if (price <= 0)
        {
            return Result<Price>.Fail(ProductErrorMessage.InvalidPrice());
        }
        
        return Result<Price>.Success(new Price(price));
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}