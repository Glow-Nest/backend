using Domain.Aggregates.Client.Values;
using Domain.Aggregates.Product.Values;
using Domain.Aggregates.ProductReview.Values;
using Domain.Common;
using OperationResult;

namespace Application.AppEntry.Commands.ProductReview;

public class CreateProductReviewCommand(ClientId reviewedBy, Rating rating, ReviewMessage reviewMessage,ProductId productId)
{
    internal readonly ClientId  reviewedBy = reviewedBy;
    internal readonly Rating rating = rating;
    internal readonly ReviewMessage reviewMessage = reviewMessage;
    internal readonly ProductId productId = productId;

    public static Result<CreateProductReviewCommand> Create(string reviewBy, int rating, string reviewMessage,
        string productId)
    {
        var listOfErrors = new List<Error>();
        
        var guidParseClientId = Guid.TryParse(reviewBy, out var clientId);
        if (!guidParseClientId)
        {
            listOfErrors.Add(GenericErrorMessage.ErrorParsingGuid());
        }
        
        var clientIdResult = ClientId.FromGuid(clientId);
        
        var guidParseProductId = Guid.TryParse(productId, out var productIdGuid);
        if (!guidParseProductId)
        {
            listOfErrors.Add(GenericErrorMessage.ErrorParsingGuid());
        }
        
        var productIdResult = ProductId.FromGuid(productIdGuid);
        
        var ratingResult = Rating.Create(rating);
        if (!ratingResult.IsSuccess)
        {
            listOfErrors.AddRange(ratingResult.Errors);
        }
        
        var reviewMessageResult = ReviewMessage.Create(reviewMessage);
        if (!reviewMessageResult.IsSuccess)
        {
            listOfErrors.AddRange(reviewMessageResult.Errors);
        }
        
        if (listOfErrors.Any())
        {
            return Result<CreateProductReviewCommand>.Fail(listOfErrors);
        }
        
        var command = new CreateProductReviewCommand(clientIdResult, ratingResult.Data, reviewMessageResult.Data,
            productIdResult);
        return Result<CreateProductReviewCommand>.Success(command);
    }
}