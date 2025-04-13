using Application.AppEntry;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using QueryContracts.QueryDispatching;

namespace WebAPI.Endpoints.Common
{
    public static class QueryEndpoint
    {
        public static class WithRequest<TRequest>
        {
            public abstract class WithResponse<TResponse> : EndpointBase
            {
                [HttpPost]
                public abstract Task<ActionResult<TResponse>> HandleAsync(TRequest request, [FromServices] IQueryDispatcher queryDispatcher);
            }

            public abstract class WithoutResponse : EndpointBase
            {
                [HttpPost]
                public abstract Task<ActionResult> HandleAsync(TRequest request, [FromServices] IQueryDispatcher queryDispatcher);
            }
        }

        public static class WithoutRequest
        {
            public abstract class WithResponse<TResponse> : EndpointBase
            {
                [HttpPost]
                public abstract Task<ActionResult<TResponse>> HandleAsync([FromServices] IQueryDispatcher queryDispatcher);
            }

            public abstract class WithoutResponse : EndpointBase
            {
                [HttpPost]
                public abstract Task<ActionResult> HandleAsync([FromServices] IQueryDispatcher queryDispatcher);
            }
        }
    }
}