using Domain.Aggregates.Client.Values;
using Domain.Aggregates.ServiceCategory.Values;
using Domain.Aggregates.ServiceReview.Values;
using Domain.Common.BaseClasses;
using Domain.Common.Contracts;
using OperationResult;

namespace Domain.Aggregates.ServiceReview;

public class ServiceReview : AggregateRoot
{
    internal ServiceReviewId ServiceReviewId { get; private set; }
    internal ClientId ClientId { get; private set; }
    internal Rating Rating { get; private set; }
    internal ServiceId ServiceId { get; private set; }
    internal ReviewMessage ReviewMessage { get; private set; }
    
    // For EFC
    private ServiceReview()
    {
    }
    
    protected ServiceReview(ServiceReviewId serviceReviewId, ClientId clientId, Rating rating, ServiceId serviceId, ReviewMessage reviewMessage)
    {
        ServiceReviewId = serviceReviewId;
        ClientId = clientId;
        Rating = rating;
        ServiceId = serviceId;
        ReviewMessage = reviewMessage;
    }

    public static async Task<Result<ServiceReview>> Create(ClientId clientId,
        Rating rating, ServiceId serviceId, ReviewMessage reviewMessage)
    {
        var reviewId = ServiceReviewId.Create();
        var review = new ServiceReview(reviewId, clientId, rating, serviceId, reviewMessage);
        return Result<ServiceReview>.Success(review);
    }
}