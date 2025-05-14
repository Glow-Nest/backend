using Application.AppEntry;
using Microsoft.AspNetCore.Mvc;
using OperationResult;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.ProductReview;

public record CreateProductReviewRequest(string ReviewById, int Rating, string ProductId, string ReviewMessage);
public class CreateProductReviewEndpoint(ICommandDispatcher commandDispatcher) : PublicWithRequest<CreateProductReviewRequest>
{
    [HttpPost("productReview/create")]
    public override async Task<ActionResult> HandleAsync(CreateProductReviewRequest request)
    {
        var commandResult = Application.AppEntry.Commands.ProductReview.CreateProductReviewCommand.Create(
            request.ReviewById,
            request.Rating,
            request.ReviewMessage,
            request.ProductId
        );
        if (!commandResult.IsSuccess)
        {
            return await Task.FromResult<ActionResult>(BadRequest(commandResult.Errors));
        }

        var dispatchResult = await commandDispatcher.DispatchAsync<Application.AppEntry.Commands.ProductReview.CreateProductReviewCommand, None>(commandResult.Data);
        return dispatchResult.IsSuccess ? Ok() : BadRequest(dispatchResult.Errors);
    }
}