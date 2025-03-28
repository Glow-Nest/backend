using Application.AppEntry;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Endpoints.Common;

public static class CommandEndpoint
{
    public static class WithRequest<TRequest>
    {
        public abstract class WithResponse<TResponse> : EndpointBase
        {
            [HttpPost]
            public abstract Task<ActionResult<TResponse>> HandleAsync(TRequest request, [FromServices] ICommandDispatcher commandDispatcher);
        }
        
        public abstract class WithoutResponse : EndpointBase
        {
            [HttpPost]
            public abstract Task<ActionResult> HandleAsync(TRequest request, [FromServices] ICommandDispatcher commandDispatcher);
        }
    }
    
    public static class WithoutRequest
    {
        public abstract class WithResponse<TResponse> : EndpointBase
        {
            [HttpPost]
            public abstract Task<ActionResult<TResponse>> HandleAsync([FromServices] ICommandDispatcher commandDispatcher);
        }
        
        public abstract class WithoutResponse : EndpointBase
        {
            [HttpPost]
            public abstract Task<ActionResult> HandleAsync([FromServices] ICommandDispatcher commandDispatcher);
        }
    }
}