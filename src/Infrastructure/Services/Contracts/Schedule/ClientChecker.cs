using Domain.Aggregates.Appointment.Contracts;
using Domain.Aggregates.Client.Values;

namespace Services.Contracts.Appointment;

public class ClientChecker : IClientChecker
{
    public Task<bool> DoesClientExistsAsync(ClientId clientId)
    {
        throw new NotImplementedException();
    }
}