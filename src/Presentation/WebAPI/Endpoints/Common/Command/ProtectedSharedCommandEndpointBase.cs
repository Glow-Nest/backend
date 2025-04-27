using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Endpoints.Common.Command;

[Authorize(Roles = "Salon Owner, Client")]
[ApiController]
[Route("api/")]
public abstract class ProtectedSharedWithRequestAndResponse<TRequest, TResponse> : CommandEndpointWithRequestAndResponse<TRequest, TResponse>
{
}

[Authorize(Roles = "Salon Owner, Client")]
[ApiController]
[Route("api/")]
public abstract class ProtectedSharedWithRequest<TRequest> : CommandEndpointWithRequest<TRequest>
{
}

[Authorize(Roles = "Salon Owner, Client")]
[ApiController]
[Route("api/")]
public abstract class ProtectedSharedWithResponse<TResponse> : CommandEndpointWithResponse<TResponse>
{
}

[Authorize(Roles = "Salon Owner, Client")]
[ApiController]
[Route("api/")]
public abstract class ProtectedSharedEndpoint : CommandEndpoint
{
}