using Domain.Common.BaseClasses;

namespace Domain.Aggregates.Product.Values;

public class ProductId : ValueObject
{
    public Guid Value { get; private set; }
    
    protected ProductId(Guid value)
    {
        Value = value;
    }
    
    public static ProductId Create() => new(Guid.NewGuid());

    public static ProductId FromGuid(Guid id) => new ProductId(id);
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}