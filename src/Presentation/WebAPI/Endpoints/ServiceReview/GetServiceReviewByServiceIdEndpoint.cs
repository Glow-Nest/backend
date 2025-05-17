using Microsoft.AspNetCore.Mvc;
using QueryContracts.Queries.ServiceReview;
using QueryContracts.QueryDispatching;
using WebAPI.Endpoints.Common.Query;
using WebAPI.Endpoints.Product;

namespace WebAPI.Endpoints.ServiceReview;

public class GetServiceReviewByServiceIdEndpoint(IQueryDispatcher queryDispatcher) : PublicQueryWithRequestAndResponse<GetServiceReviewByServiceId.Query,GetServiceReviewByServiceId.Answer>
{
    [HttpPost("serviceReview/{ServiceId}")]
    public override async Task<ActionResult<GetServiceReviewByServiceId.Answer>> HandleAsync([FromRoute] GetServiceReviewByServiceId.Query request)
    {
        var dispatchResult = await queryDispatcher.DispatchAsync(request);
        if (!dispatchResult.IsSuccess)
        {
            return BadRequest(dispatchResult.Errors);
        }

        return Ok(dispatchResult.Data);
    }
}