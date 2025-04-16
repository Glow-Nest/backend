using Domain.Aggregates.Client.Values;

namespace Domain.Aggregates.Appointment.Contracts;

public interface IClientChecker
{
    Task<bool> DoesClientExistsAsync(ClientId clientId);
}