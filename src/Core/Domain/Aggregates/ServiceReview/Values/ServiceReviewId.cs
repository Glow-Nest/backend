using Domain.Common.BaseClasses;

namespace Domain.Aggregates.ServiceReview.Values;

public class ServiceReviewId : ValueObject
{
    public Guid Value { get; private set; }

    protected ServiceReviewId(Guid value)
    {
        Value = value;
    }

    public static ServiceReviewId Create() => new(Guid.NewGuid());

    public static ServiceReviewId FromGuid(Guid id) => new ServiceReviewId(id);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}