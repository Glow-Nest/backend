using Domain.Common.BaseClasses;
using OperationResult;

namespace Domain.Aggregates.ServiceReview.Values;

public class Rating : ValueObject
{
    internal int Value { get; private set; }
    
    private Rating(int value)
    {
        Value = value;
    }
    
    public static Result<Rating> Create(int value)
    {
        if (value == 0)
        {
            return Result<Rating>.Fail(ServiceReviewErrorMessage.RatingCannotBeEmpty());
        }
        if (value is < 1 or > 5)
        {
            return Result<Rating>.Fail(ServiceReviewErrorMessage.RatingCannotBeMoreThan5Characters());
        }

        return Result<Rating>.Success(new Rating(value));
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}