using Microsoft.AspNetCore.Mvc;
using QueryContracts.Queries.Service;
using QueryContracts.QueryDispatching;
using WebAPI.Endpoints.Common.Query;

namespace WebAPI.Endpoints.Category;

public class GetAllCategoryWithServiceEndpoint : PublicQueryWithRequestAndResponse<GetAllCategoriesWithServices.Query, GetAllCategoriesWithServices.Answer>
{
    [HttpPost("categories/services/all")]
    public override async Task<ActionResult<GetAllCategoriesWithServices.Answer>> HandleAsync(GetAllCategoriesWithServices.Query request, IQueryDispatcher queryDispatcher)
    {
        var dispatchResult = await queryDispatcher.DispatchAsync(request);
        if (!dispatchResult.IsSuccess)
        {
            return BadRequest(dispatchResult.Errors);
        }

        return Ok(dispatchResult.Data);
    }
}