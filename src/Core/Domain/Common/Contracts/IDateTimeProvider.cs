namespace Domain.Common.Contracts;

public interface IDateTimeProvider
{
    DateTime GetNow();
}