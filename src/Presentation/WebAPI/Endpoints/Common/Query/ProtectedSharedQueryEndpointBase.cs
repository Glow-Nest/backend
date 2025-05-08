using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Endpoints.Common.Query;

[Authorize(Roles = "Salon Owner, Client")]
[ApiController]
[Route("api/")]
public abstract class ProtectedSharedQueryWithRequestAndResponse<TRequest, TResponse>
    : QueryEndpointWithRequestAndResponse<TRequest, TResponse>
{
}

[Authorize(Roles = "Salon Owner, Client")]
[ApiController]
[Route("api/")]
public abstract class ProtectedSharedQueryWithRequest<TRequest>
    : QueryEndpointWithRequest<TRequest>
{
}

[Authorize(Roles = "Salon Owner, Client")]
[ApiController]
[Route("api/")]
public abstract class ProtectedSharedQueryWithResponse<TResponse>
    : QueryEndpointWithResponse<TResponse>
{
}

[Authorize(Roles = "Salon Owner, Client")]
[ApiController]
[Route("api/")]
public abstract class ProtectedSharedQueryEndpoint
    : QueryEndpoint
{
}