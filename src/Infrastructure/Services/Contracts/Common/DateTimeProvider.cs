using Domain.Aggregates.Client.Contracts;

namespace Services.Contracts.Client;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime GetNow()
    {
        return DateTime.Now;
    }
}