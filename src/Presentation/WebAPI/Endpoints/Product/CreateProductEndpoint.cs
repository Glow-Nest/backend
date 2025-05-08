using Application.AppEntry;
using Application.AppEntry.Commands.Product;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.Product;

public record CreateProductRequest(string Name, string Description, double Price, string ImageUrl, int InventoryCount);

public class CreateProductEndpoint(ICommandDispatcher commandDispatcher) : ProtectedOwnerWithRequest<CreateProductRequest>
{
    [HttpPost("product/create")]
    public override async Task<ActionResult> HandleAsync(CreateProductRequest request)
    {
        var commandResult = CreateProductCommand.Create(request.Name, request.Price, request.Description,
            request.ImageUrl, request.InventoryCount);

        if (!commandResult.IsSuccess)
        {
            return BadRequest(commandResult.Errors);
        }

        var dispatchResult = await commandDispatcher.DispatchAsync(commandResult.Data);
        return dispatchResult.IsSuccess ? Ok() : BadRequest(dispatchResult.Errors);
    }
}