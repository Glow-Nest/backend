using Domain.Common.BaseClasses;
using OperationResult;

namespace Domain.Aggregates.ServiceReview.Values;

public class ReviewMessage : ValueObject
{
    
    public string Value { get; }
    
    public ReviewMessage(string value)
    {
        Value = value;
    }
    
    public static Result<ReviewMessage> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return Result<ReviewMessage>.Fail(ServiceReviewErrorMessage.ReviewCommentCannotBeEmpty());
        }

        return Result<ReviewMessage>.Success(new ReviewMessage(value));
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}