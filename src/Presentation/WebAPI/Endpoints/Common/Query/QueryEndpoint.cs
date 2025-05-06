using Microsoft.AspNetCore.Mvc;
using QueryContracts.QueryDispatching;

namespace WebAPI.Endpoints.Common.Query;

public abstract class QueryEndpointWithRequestAndResponse<TRequest, TResponse> : ControllerBase
{
    [HttpGet]
    public abstract Task<ActionResult<TResponse>> HandleAsync(TRequest request);
}

public abstract class QueryEndpointWithRequest<TRequest> : ControllerBase
{
    [HttpGet]
    public abstract Task<ActionResult> HandleAsync(TRequest request);
}

public abstract class QueryEndpointWithResponse<TResponse> : ControllerBase
{
    [HttpGet]
    public abstract Task<ActionResult<TResponse>> HandleAsync();
}

public abstract class QueryEndpoint : ControllerBase
{
    [HttpGet]
    public abstract Task<ActionResult> HandleAsync();
}