using Microsoft.AspNetCore.Mvc;
using QueryContracts.Queries.Service;
using QueryContracts.QueryDispatching;
using WebAPI.Endpoints.Common.Query;

namespace WebAPI.Endpoints.Service;

public class GetAllServiceEndpoint : PublicQueryWithRequestAndResponse<GetAllServiceQuery, GetAllServicesResponse>
{
    [HttpPost("service/all")]
    public override async Task<ActionResult<GetAllServicesResponse>> HandleAsync([FromQuery]GetAllServiceQuery request, IQueryDispatcher queryDispatcher)
    {
        var dispatchResult = await queryDispatcher.DispatchAsync(request);
        if (dispatchResult.IsSuccess)
        {
            return await Task.FromResult<ActionResult<GetAllServicesResponse>>(Ok(dispatchResult.Data));
        }
        return await Task.FromResult<ActionResult<GetAllServicesResponse>>(BadRequest(dispatchResult.Errors));
    }
}