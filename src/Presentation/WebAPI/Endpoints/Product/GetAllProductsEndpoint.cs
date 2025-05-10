using Microsoft.AspNetCore.Mvc;
using QueryContracts.Contracts;
using QueryContracts.Queries.Product;
using QueryContracts.QueryDispatching;
using WebAPI.Endpoints.Common.Query;

namespace WebAPI.Endpoints.Product;

public class GetAllProductsEndpoint(IQueryDispatcher queryDispatcher) : PublicQueryWithRequestAndResponse<GetAllProductsEndpoint.Request, GetAllProductsEndpoint.Response>
{
    public record ProductResponseDto(string ProductId, string Name , double Price, string ImageUrl);
    public new record Request(int Page, int PageSize) : IQuery<Response>;
    public new record Response(List<ProductResponseDto> Products, int TotalCount);
    
    [HttpPost("products/all")]
    public override async Task<ActionResult<Response>> HandleAsync([FromBody] Request request)
    {
        var query = new GetAllProductsQuery.Query(request.Page, request.PageSize);
        var answerResult = await queryDispatcher.DispatchAsync(query);
        if (!answerResult.IsSuccess)
        {
            return BadRequest(answerResult);
        }
        
        var products = answerResult.Data.Products
            .Select(p => new ProductResponseDto(p.ProductId, p.ProductName, p.Price, p.ImageUrl))
            .ToList();
        var response = new Response(products, answerResult.Data.TotalCount);
        return Ok(response);
    }
} 