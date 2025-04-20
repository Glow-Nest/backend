using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Endpoints.Common.Query;

[ApiController]
[Route("api")]
public abstract class PublicQueryWithRequestAndResponse<TRequest, TResponse>
    : QueryEndpointWithRequestAndResponse<TRequest, TResponse>
{
}

[ApiController]
[Route("api")]
public abstract class PublicQueryWithRequest<TRequest>
    : QueryEndpointWithRequest<TRequest>
{
}

[ApiController]
[Route("api")]
public abstract class PublicQueryWithResponse<TResponse>
    : QueryEndpointWithResponse<TResponse>
{
}

[ApiController]
[Route("api")]
public abstract class PublicQueryEndpoint
    : QueryEndpoint
{
}