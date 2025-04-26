using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Endpoints.Common.Command;

[Authorize(Roles = "Salon Owner")]
[ApiController]
[Route("api/owner")]
public abstract class ProtectedOwnerWithRequestAndResponse<TRequest, TResponse> : CommandEndpointWithRequestAndResponse<TRequest, TResponse>
{
}

[Authorize(Roles = "Salon Owner")]
[ApiController]
[Route("api/owner")]
public abstract class ProtectedOwnerWithRequest<TRequest> : CommandEndpointWithRequest<TRequest>
{
}

[Authorize(Roles = "Salon Owner")]
[ApiController]
[Route("api/owner")]
public abstract class ProtectedOwnerWithResponse<TResponse> : CommandEndpointWithResponse<TResponse>
{
}

[Authorize(Roles = "Salon Owner")]
[ApiController]
[Route("api/owner")]
public abstract class ProtectedOwnerEndpoint : CommandEndpoint
{
}