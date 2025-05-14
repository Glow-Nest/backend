using OperationResult;

namespace Domain.Aggregates.ServiceReview;

public class ServiceReviewErrorMessage
{
    public static Error RatingCannotBeEmpty() => new Error("ServiceReview.RatingCannotBeEmpty", "Rating cannot be empty");

    public static Error RatingCannotBeMoreThan5Characters() => new Error("ServiceReview.RatingCannotBeMoreThan5Characters", "Rating cannot be more than 5 characters");

    public static Error ReviewCommentCannotBeEmpty() => new Error("ServiceReview.ReviewCommentCannotBeEmpty", "Review ReviewMessage cannot be empty");

    public static Error ServiceReviewNotFound() => new Error("ServiceReview.ServiceReviewNotFound", "Service review not found");
}