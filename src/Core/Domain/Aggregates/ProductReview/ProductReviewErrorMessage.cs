using OperationResult;

namespace Domain.Aggregates.ProductReview;

public class ProductReviewErrorMessage
{
    public static Error RatingCannotBeEmpty() => new Error("ProductReview.RatingCannotBeEmpty", "Rating cannot be empty");

    public static Error RatingCannotBeMoreThan5Characters() => new Error("ProductReview.RatingCannotBeMoreThan5Characters", "Rating cannot be more than 5 characters");

    public static Error ReviewCommentCannotBeEmpty() => new Error("ProductReview.ReviewCommentCannotBeEmpty", "Review comment cannot be empty");
}