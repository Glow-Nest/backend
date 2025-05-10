using Microsoft.AspNetCore.Mvc;
using QueryContracts.Queries.Product;
using QueryContracts.QueryDispatching;
using WebAPI.Endpoints.Common.Query;

namespace WebAPI.Endpoints.Product;

public class GetProductByIdEndpoint(IQueryDispatcher queryDispatcher) : PublicQueryWithRequestAndResponse<GetProductByIdEndpoint.Request, GetProductByIdEndpoint.Response>
{
    public new record Request(string ProductId);
    public new record Response(string ProductId, string Name, string Description, double Price, string ImageUrl, int InventoryCount);
    
    [HttpPost("products/{productId}")]
    public override async Task<ActionResult<Response>> HandleAsync([FromRoute] Request request)
    {
        var queryResult = new GetProductByIdQuery.Query(request.ProductId);

        var dispatchResult = await queryDispatcher.DispatchAsync(queryResult);
        if (!dispatchResult.IsSuccess)
        {
            return BadRequest(dispatchResult.Errors);
        }

        var product = dispatchResult.Data;
        var response = new Response(product.ProductId, product.ProductName, product.Description, product.Price, product.ImageUrl, product.InventoryCount);
        
        return Ok(response);
    }
}