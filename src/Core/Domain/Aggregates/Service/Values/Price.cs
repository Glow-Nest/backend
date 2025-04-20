using Domain.Common.BaseClasses;
using Domain.Common.OperationResult;

namespace Domain.Aggregates.Service.Values;

public class Price : ValueObject
{
    
    internal double Value { get; }
    
    private Price(double value)
    {
        Value = value;
    }
    
    public static Result<Price> Create(double price)
    {
        if (price <= 0)
        {
            return Result<Price>.Fail(ServiceErrorMessage.InvalidServicePrice());
        }
        
        return Result<Price>.Success(new Price(price));
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}