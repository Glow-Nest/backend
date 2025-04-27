using Microsoft.AspNetCore.Mvc;
using QueryContracts.Queries.Service;
using QueryContracts.QueryDispatching;
using WebAPI.Endpoints.Common.Query;

namespace WebAPI.Endpoints.Category;

public class GetAllCategoryEndpoint : PublicQueryWithRequestAndResponse<GetAllCategory.Query, GetAllCategory.Answer>
{
    [HttpPost("category/all")]
    public override async Task<ActionResult<GetAllCategory.Answer>> HandleAsync(GetAllCategory.Query request, IQueryDispatcher queryDispatcher)
    {
        var dispatchResult = await queryDispatcher.DispatchAsync(request);
        if (!dispatchResult.IsSuccess)
        {
            return BadRequest(dispatchResult.Errors);
        }

        return Ok(dispatchResult.Data);
    }
}