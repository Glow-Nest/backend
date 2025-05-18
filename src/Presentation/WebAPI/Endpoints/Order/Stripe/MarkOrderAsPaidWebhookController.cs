using Application.AppEntry;
using Application.AppEntry.Commands.Order;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OperationResult;
using Services.Stripe;
using Stripe;
using Stripe.Checkout;
using WebAPI.Endpoints.Common.Command;

namespace WebAPI.Endpoints.Order.Stripe;

public class MarkOrderAsPaidWebhookController(ICommandDispatcher commandDispatcher, IOptions<StripeSettings> options)
    : PublicWithResponse<None>
{
    private readonly ICommandDispatcher _commandDispatcher = commandDispatcher;
    private readonly string _webhookSecret = options.Value.WebhookSecret;

    [HttpPost("webhook/stripe/mark-order-as-paid")]
    public override async Task<ActionResult<None>> HandleAsync()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

        Event stripeEvent;

        try
        {
            stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], _webhookSecret);
        }
        catch (Exception e)
        {
            Console.WriteLine("Stripe webhook signature verification failed." + e.Message);
            return BadRequest();
        }

        if (stripeEvent.Type == EventTypes.CheckoutSessionCompleted)
        {
            var session = stripeEvent.Data.Object as Session;

            var markOrderPaidResult =
                MarkOrderAsPaidCommand.Create(session?.Metadata["OrderId"], session?.PaymentIntentId);
            if (!markOrderPaidResult.IsSuccess)
            {
                return BadRequest(markOrderPaidResult.Errors);
            }

            var command = markOrderPaidResult.Data;
            var result = await _commandDispatcher.DispatchAsync<MarkOrderAsPaidCommand, None>(command);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Errors);
            }
        }
        
        return Ok();
    }
}