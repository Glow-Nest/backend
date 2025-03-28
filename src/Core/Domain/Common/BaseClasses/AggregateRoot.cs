namespace Domain.Common.BaseClasses;

public abstract class AggregateRoot<TId>
{
    public TId Id { get; protected set; } = default!;
}