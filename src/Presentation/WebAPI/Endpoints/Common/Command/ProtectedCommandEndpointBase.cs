using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Endpoints.Common.Command;

[Authorize(Roles = "Salon Owner")]
[ApiController]
[Route("api/owner")]
public abstract class ProtectedWithRequestAndResponse<TRequest, TResponse> : CommandEndpointWithRequestAndResponse<TRequest, TResponse>
{
}

[Authorize(Roles = "Salon Owner")]
[ApiController]
[Route("api/owner")]
public abstract class ProtectedWithRequest<TRequest> : CommandEndpointWithRequest<TRequest>
{
}

[Authorize(Roles = "Salon Owner")]
[ApiController]
[Route("api/owner")]
public abstract class ProtectedWithResponse<TResponse> : CommandEndpointWithResponse<TResponse>
{
}

[Authorize(Roles = "Salon Owner")]
[ApiController]
[Route("api/owner")]
public abstract class ProtectedEndpoint : CommandEndpoint
{
}