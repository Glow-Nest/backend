using Application.AppEntry;
using Application.AppEntry.Commands.ServiceReview;
using Microsoft.AspNetCore.Mvc;
using OperationResult;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.ServiceReview;

public record CreateServiceReviewRequest(string ReviewById, int Rating, string ServiceId, string Comment);
public class CreateServiceReviewEndpoint(ICommandDispatcher commandDispatcher) : PublicWithRequest<CreateServiceReviewRequest>
{
    [HttpPost("serviceReview/create")]
    public override async Task<ActionResult> HandleAsync(CreateServiceReviewRequest request)
    {
        var commandResult = CreateServiceReviewCommand.Create(request.ReviewById, request.Rating,request.Comment, request.ServiceId);
        if (!commandResult.IsSuccess)
        {
            return await Task.FromResult<ActionResult>(BadRequest(commandResult.Errors));
        }

        var dispatchResult = await commandDispatcher.DispatchAsync<CreateServiceReviewCommand, None>(commandResult.Data);
        return dispatchResult.IsSuccess ? Ok() : BadRequest(dispatchResult.Errors);
    }
}