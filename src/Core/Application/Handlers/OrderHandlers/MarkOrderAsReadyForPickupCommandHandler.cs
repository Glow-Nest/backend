using Application.AppEntry;
using Application.AppEntry.Commands.Order;
using OperationResult;

namespace Application.Handlers.OrderHandlers;

public class MarkOrderAsReadyForPickupCommandHandler : ICommandHandler<MarkOrderAsReadyForPickupCommand, None>
{
    public Task<Result<None>> HandleAsync(MarkOrderAsReadyForPickupCommand command)
    {
        throw new NotImplementedException();
    }
}