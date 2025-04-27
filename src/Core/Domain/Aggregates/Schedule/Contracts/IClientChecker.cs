using Domain.Aggregates.Client.Values;

namespace Domain.Aggregates.Schedule.Contracts;

public interface IClientChecker
{
    Task<bool> DoesClientExistsAsync(ClientId id);
}