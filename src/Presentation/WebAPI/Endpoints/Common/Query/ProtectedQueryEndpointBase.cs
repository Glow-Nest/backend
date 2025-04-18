using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Endpoints.Common.Query;

[Authorize(Roles = "Salon Owner")]
[ApiController]
[Route("api/owner")]
public abstract class ProtectedQueryWithRequestAndResponse<TRequest, TResponse>
    : QueryEndpointWithRequestAndResponse<TRequest, TResponse>
{
}

[Authorize(Roles = "Salon Owner")]
[ApiController]
[Route("api/owner")]
public abstract class ProtectedQueryWithRequest<TRequest>
    : QueryEndpointWithRequest<TRequest>
{
}

[Authorize(Roles = "Salon Owner")]
[ApiController]
[Route("api/owner")]
public abstract class ProtectedQueryWithResponse<TResponse>
    : QueryEndpointWithResponse<TResponse>
{
}

[Authorize(Roles = "Salon Owner")]
[ApiController]
[Route("api/owner")]
public abstract class ProtectedQueryEndpoint
    : QueryEndpoint
{
}