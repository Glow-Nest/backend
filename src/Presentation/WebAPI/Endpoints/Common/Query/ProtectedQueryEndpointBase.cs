using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Endpoints.Common.Query;

[Authorize(Roles = "SalonOwner")]
[ApiController]
[Route("api/owner")]
public abstract class ProtectedQueryWithRequestAndResponse<TRequest, TResponse>
    : QueryEndpointWithRequestAndResponse<TRequest, TResponse>
{
}

[Authorize(Roles = "SalonOwner")]
[ApiController]
[Route("api/owner")]
public abstract class ProtectedQueryWithRequest<TRequest>
    : QueryEndpointWithRequest<TRequest>
{
}

[Authorize(Roles = "SalonOwner")]
[ApiController]
[Route("api/owner")]
public abstract class ProtectedQueryWithResponse<TResponse>
    : QueryEndpointWithResponse<TResponse>
{
}

[Authorize(Roles = "SalonOwner")]
[ApiController]
[Route("api/owner")]
public abstract class ProtectedQueryEndpoint
    : QueryEndpoint
{
}