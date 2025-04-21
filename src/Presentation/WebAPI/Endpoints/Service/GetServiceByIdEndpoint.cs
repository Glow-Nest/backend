using Microsoft.AspNetCore.Mvc;
using QueryContracts.Queries.Service;
using QueryContracts.QueryDispatching;
using WebAPI.Endpoints.Common.Query;

namespace WebAPI.Endpoints.Service;

public class GetServiceByIdEndpoint : PublicQueryWithRequestAndResponse<GetServiceByIdQuery, GetServiceByIdResponse>
{
    [HttpPost("service/{ServiceId:guid}")]
    public override async Task<ActionResult<GetServiceByIdResponse>> HandleAsync([FromRoute]GetServiceByIdQuery request, IQueryDispatcher queryDispatcher)
    {
        var result = await queryDispatcher.DispatchAsync(request);

        if (result.IsSuccess)
            return Ok(result.Data);

        return BadRequest(result.Errors);
    }
}