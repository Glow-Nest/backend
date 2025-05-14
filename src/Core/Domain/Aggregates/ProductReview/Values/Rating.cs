using Domain.Aggregates.Product;
using Domain.Common.BaseClasses;
using OperationResult;

namespace Domain.Aggregates.ProductReview.Values;

public class Rating: ValueObject
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
            return Result<Rating>.Fail(ProductReviewErrorMessage.RatingCannotBeEmpty());
        }
        if (value is < 1 or > 5)
        {
            return Result<Rating>.Fail(ProductReviewErrorMessage.RatingCannotBeMoreThan5Characters());
        }

        return Result<Rating>.Success(new Rating(value));
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
}