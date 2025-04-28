using Microsoft.AspNetCore.Mvc;
using QueryContracts.Queries.Service;
using QueryContracts.QueryDispatching;
using WebAPI.Endpoints.Common.Query;

namespace WebAPI.Endpoints.Category;

public class GetAllCategoryWithServiceEndpoint : PublicQueryWithResponse<GetAllCategoriesWithServices.Answer>
{
    [HttpPost("categories/services/all")]
    public override async Task<ActionResult<GetAllCategoriesWithServices.Answer>> HandleAsync(IQueryDispatcher queryDispatcher)
    {
        var query = new GetAllCategoriesWithServices.Query();

        var dispatchResult = await queryDispatcher.DispatchAsync(query);
        if (!dispatchResult.IsSuccess)
        {
            return BadRequest(dispatchResult.Errors);
        }

        return Ok(dispatchResult.Data);
    }
}