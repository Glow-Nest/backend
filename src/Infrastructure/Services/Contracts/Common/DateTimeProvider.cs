using Domain.Aggregates.Client.Contracts;

namespace Services.Contracts.Common;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime GetNow()
    {
        return DateTime.Now;
    }
}