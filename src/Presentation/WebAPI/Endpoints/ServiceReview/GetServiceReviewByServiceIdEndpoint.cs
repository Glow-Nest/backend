using Microsoft.AspNetCore.Mvc;
using QueryContracts.Queries.ServiceReview;
using QueryContracts.QueryDispatching;
using WebAPI.Endpoints.Common.Query;
using WebAPI.Endpoints.Product;

namespace WebAPI.Endpoints.ServiceReview;

public class GetServiceReviewByServiceIdEndpoint(IQueryDispatcher queryDispatcher) : PublicQueryWithRequestAndResponse<GetServiceReviewByServiceIdEndpoint.Request,GetServiceReviewByServiceIdEndpoint.Response>
{
    public new record Request(string ServiceId);
    public new record Response(string ServiceReviewId, string UserId, string ServiceId, int Rating, string ReviewMessage);
    
    [HttpPost("serviceReview/{ServiceId}")]
    public override async Task<ActionResult<Response>> HandleAsync([FromRoute] Request request)
    {
        var queryResult = new GetServiceReviewByServiceId.Query(request.ServiceId);

        var dispatchResult = await queryDispatcher.DispatchAsync(queryResult);
        if (!dispatchResult.IsSuccess)
        {
            return BadRequest(dispatchResult.Errors);
        }

        var reviews = dispatchResult.Data.Reviews;
        var response = reviews.Select(review => new Response(review.ServiceReviewId, review.UserId, review.ServiceId, review.Rating, review.ReviewMessage)).ToList();
        
        return Ok(response);
    }
}