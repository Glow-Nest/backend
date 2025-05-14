using Microsoft.AspNetCore.Mvc;
using QueryContracts.Queries.Product;
using QueryContracts.QueryDispatching;
using WebAPI.Endpoints.Common.Query;

namespace WebAPI.Endpoints.Product;

public class GetProductByNameEndpoint(IQueryDispatcher queryDispatcher) : PublicQueryWithRequestAndResponse<GetProductByNameEndpoint.Request, GetProductByNameEndpoint.Response>
{
    public record ProductResponseDto(string ProductId, string Name, double Price,  string ImageUrl);
    public new record Request(string ProductName);
    public new record Response(List<ProductResponseDto> Products);
    
    [HttpPost("products/name/{productName}")]
    public override async Task<ActionResult<Response>> HandleAsync([FromRoute] Request request)
    {
        var query = new GetProductByNameQuery.Query(request.ProductName);
        var result = await queryDispatcher.DispatchAsync(query);

        if (!result.IsSuccess)
        {
            return BadRequest(result.Errors);
        }
        var products = result.Data
            .Select(p => new ProductResponseDto(p.ProductId, p.ProductName, p.Price, p.ImageUrl))
            .ToList();
        var response = new Response(products);
        return Ok(response);
    }

}