using Domain.Aggregates.Client.Values;
using Domain.Aggregates.ServiceCategory.Values;
using Domain.Aggregates.ServiceReview.Values;
using Domain.Common;
using OperationResult;

namespace Application.AppEntry.Commands.ServiceReview;

public class CreateServiceReviewCommand(ClientId reviewBy, Rating rating, ReviewMessage reviewMessage,ServiceId serviceId)
{
    internal readonly ClientId reviewedBy = reviewBy;
    internal readonly Rating rating = rating;
    internal readonly ReviewMessage ReviewMessage = reviewMessage;
    internal readonly ServiceId serviceId = serviceId;
    
    public static Result<CreateServiceReviewCommand> Create(string reviewBy, int rating, string comment, string serviceId)
    {
        var listOfErrors = new List<Error>();
        
        var guidParseClientId = Guid.TryParse(reviewBy, out var clientId);
        if (!guidParseClientId)
        {
            listOfErrors.Add(GenericErrorMessage.ErrorParsingGuid());
        }
        var clientIdResult = ClientId.FromGuid(clientId);
        
        var guidParseServiceId = Guid.TryParse(serviceId, out var serviceIdGuid);
        if (!guidParseServiceId)
        {
            listOfErrors.Add(GenericErrorMessage.ErrorParsingGuid());
        }
        var serviceIdResult = ServiceId.FromGuid(serviceIdGuid);
        
        var ratingResult = Rating.Create(rating);
        if (!ratingResult.IsSuccess)
        {
            listOfErrors.AddRange(ratingResult.Errors);
        }
        
        var commentResult = ReviewMessage.Create(comment);
        if (!commentResult.IsSuccess)
        {
            listOfErrors.AddRange(commentResult.Errors);
        }
        
        if (listOfErrors.Any())
        {
            return Result<CreateServiceReviewCommand>.Fail(listOfErrors);
        }
        
        var command = new CreateServiceReviewCommand(clientIdResult,ratingResult.Data,commentResult.Data,serviceIdResult);
        
        return Result<CreateServiceReviewCommand>.Success(command);
    }
    
}