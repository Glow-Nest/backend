using Domain.Common.BaseClasses;

namespace Domain.Aggregates.ProductReview.Values;

public class ProductReviewId : ValueObject
{
    public Guid Value { get; private set; }

    protected ProductReviewId(Guid value)
    {
        Value = value;
    }

    public static ProductReviewId Create() => new(Guid.NewGuid());

    public static ProductReviewId FromGuid(Guid id) => new ProductReviewId(id);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}