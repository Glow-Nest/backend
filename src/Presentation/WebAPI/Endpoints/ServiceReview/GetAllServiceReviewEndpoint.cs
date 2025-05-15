using Microsoft.AspNetCore.Mvc;
using QueryContracts.Queries.ServiceReview;
using QueryContracts.QueryDispatching;
using WebAPI.Endpoints.Common.Query;
using WebAPI.Endpoints.Product;

namespace WebAPI.Endpoints.ServiceReview;

public class GetAllServiceReviewEndpoint(IQueryDispatcher queryDispatcher) : PublicQueryWithRequestAndResponse<GetAllServiceReview.Query,GetAllServiceReview.Answer>
{
    [HttpPost("serviceReview/all")]
    public override async Task<ActionResult<GetAllServiceReview.Answer>> HandleAsync(GetAllServiceReview.Query request)
    {
        var dispatchResult = await queryDispatcher.DispatchAsync(request);
        if (!dispatchResult.IsSuccess)
        {
            return BadRequest(dispatchResult.Errors);
        }

        return Ok(dispatchResult.Data);
    }
}