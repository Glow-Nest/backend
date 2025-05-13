using Domain.Common.BaseClasses;
using OperationResult;

namespace Domain.Aggregates.Product.Values;

public class InventoryCount : ValueObject
{
    public int Value { get; private set; }
    
    private InventoryCount(int value)
    {
        Value = value;
    }
    
    public static Result<InventoryCount> Create(int value)
    {
        if (value < 0)
        {
            return Result<InventoryCount>.Fail(ProductErrorMessage.InvalidInventoryCount());
        }
        
        return Result<InventoryCount>.Success(new InventoryCount(value));
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}