using Domain.Aggregates.ServiceCategory.Values;

namespace Application.AppEntry.Commands.ServiceCategory.UpdateCategoryCommand;

public abstract class CategoryUpdateCommandBase
{
    internal CategoryId Id { get; }

    protected CategoryUpdateCommandBase(CategoryId id)
    {
        Id= id;
    }
}