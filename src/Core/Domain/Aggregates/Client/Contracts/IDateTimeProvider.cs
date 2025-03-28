namespace Domain.Aggregates.Client.Contracts;

public interface IDateTimeProvider
{
    DateTime GetNow();
}