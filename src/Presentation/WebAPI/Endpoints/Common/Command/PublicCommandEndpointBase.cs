using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Endpoints.Common.Command;

[ApiController]
[Route("api")]
public abstract class PublicWithRequestAndResponse<TRequest, TResponse> : CommandEndpointWithRequestAndResponse<TRequest, TResponse>
{
}

[ApiController]
[Route("api")]
public abstract class PublicWithRequest<TRequest> : CommandEndpointWithRequest<TRequest>
{
}

[ApiController]
[Route("api")]
public abstract class PublicWithResponse<TResponse> : CommandEndpointWithResponse<TResponse>
{
}

[ApiController]
[Route("api")]
public abstract class PublicEndpoint : CommandEndpoint
{
}