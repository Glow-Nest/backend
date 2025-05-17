namespace Services.Stripe;

public class StripeSettings
{
    public string SecretKey { get; init; } = default!;
    public string PublishableKey { get; init; } = default!;
    public string WebhookSecret { get; init; } = default!;
}