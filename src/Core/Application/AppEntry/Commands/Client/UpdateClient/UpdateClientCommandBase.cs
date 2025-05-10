using Domain.Aggregates.Client.Values;

namespace Application.AppEntry.Commands.Client.UpdateClient;

public class UpdateClientCommandBase
{
    internal ClientId Id { get; }
    
    protected UpdateClientCommandBase(ClientId clientId)
    {
        Id = clientId;
    }
}