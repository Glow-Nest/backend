namespace Domain.Aggregates.Client.Contracts;

public interface IEmailUniqueChecker
{
    Task<bool> IsEmailUniqueAsync(string email);
}