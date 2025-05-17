using Microsoft.AspNetCore.Mvc;
using QueryContracts.Queries.User;
using QueryContracts.QueryDispatching;
using WebAPI.Endpoints.Common.Query;

namespace WebAPI.Endpoints.Client;

public class GetFullNameByClientIdEndpoint(IQueryDispatcher queryDispatcher) : PublicQueryWithRequestAndResponse<GetFullNameByUserId.Query, GetFullNameByUserId.Answer>
{
    [HttpPost("clients/{ClientId}")]
    public override async Task<ActionResult<GetFullNameByUserId.Answer>> HandleAsync([FromRoute] GetFullNameByUserId.Query request)
    {
        var dispatchResult = await queryDispatcher.DispatchAsync(request);
        if (!dispatchResult.IsSuccess)
        {
            return BadRequest(dispatchResult.Errors);
        }

        return Ok(dispatchResult.Data);
    }
}