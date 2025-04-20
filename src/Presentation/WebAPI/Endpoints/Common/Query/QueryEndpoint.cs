using Microsoft.AspNetCore.Mvc;
using QueryContracts.QueryDispatching;

namespace WebAPI.Endpoints.Common.Query;

public abstract class QueryEndpointWithRequestAndResponse<TRequest, TResponse> : ControllerBase
{
    [HttpGet]
    public abstract Task<ActionResult<TResponse>> HandleAsync(
        TRequest request,
        [FromServices] IQueryDispatcher queryDispatcher
    );
}

public abstract class QueryEndpointWithRequest<TRequest> : ControllerBase
{
    [HttpGet]
    public abstract Task<ActionResult> HandleAsync(
        TRequest request,
        [FromServices] IQueryDispatcher queryDispatcher
    );
}

public abstract class QueryEndpointWithResponse<TResponse> : ControllerBase
{
    [HttpGet]
    public abstract Task<ActionResult<TResponse>> HandleAsync(
        [FromServices] IQueryDispatcher queryDispatcher
    );
}

public abstract class QueryEndpoint : ControllerBase
{
    [HttpGet]
    public abstract Task<ActionResult> HandleAsync(
        [FromServices] IQueryDispatcher queryDispatcher
    );
}