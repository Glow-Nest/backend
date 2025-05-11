using Microsoft.AspNetCore.Mvc;
using QueryContracts.Queries.Product;
using QueryContracts.QueryDispatching;
using WebAPI.Endpoints.Common.Query;

namespace WebAPI.Endpoints.Product;

public class GetProductByNameEndpoint(IQueryDispatcher queryDispatcher) : PublicQueryWithRequestAndResponse<GetProductByNameEndpoint.Request, GetProductByNameEndpoint.Response>
{
    public new record Request(string ProductName);
    public new record Response(string ProductId, string Name);
    
    [HttpPost("products/name/{productName}")]
    public override async Task<ActionResult<Response>> HandleAsync([FromRoute] Request request)
    {
        var queryResult = new GetProductByNameQuery.Query(request.ProductName);

        var dispatchResult = await queryDispatcher.DispatchAsync(queryResult);
        if (!dispatchResult.IsSuccess)
        {
            return BadRequest(dispatchResult.Errors);
        }

        var product = dispatchResult.Data;
        var response = new Response(product.ProductId, product.ProductName);
        
        return Ok(response);
    }
}