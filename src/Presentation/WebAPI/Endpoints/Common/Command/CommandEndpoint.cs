using Application.AppEntry;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Endpoints.Common.Command;

public abstract class CommandEndpointWithRequestAndResponse<TRequest, TResponse> : ControllerBase
{
    [HttpPost]
    public abstract Task<ActionResult<TResponse>> HandleAsync(
        TRequest request);
}

public abstract class CommandEndpointWithRequest<TRequest> : ControllerBase
{
    [HttpPost]
    public abstract Task<ActionResult> HandleAsync(
        TRequest request);
}

public abstract class CommandEndpointWithResponse<TResponse> : ControllerBase
{
    [HttpPost]
    public abstract Task<ActionResult<TResponse>> HandleAsync();
}

public abstract class CommandEndpoint  : ControllerBase
{
    [HttpPost]
    public abstract Task<ActionResult> HandleAsync();
}