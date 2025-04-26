using Domain.Common.OperationResult;

namespace Domain.Aggregates.ServiceCategory;

public class ServiceCategoryErrorMessage
{
    public static Error ServiceNotFound() => new("Service.ServiceNotFound", "Service not found.");
    public static Error NoServicesSelected() => new("Service.NoServiceSelected", "No service selected.");
    public static Error EmptyServiceName() => new("Service.EmptyServiceName", "Service name cannot be empty.");
    public static Error EmptyServiceDescription() => new("Service.EmptyServiceDescription", "Service description cannot be empty.");
    public static Error InvalidServicePrice() => new("Service.InvalidPrice", "Invalid service price.");
    public static Error EmptyServiceMediaUrl() => new("Service.EmptyServiceMediaUrl", "Service media URL cannot be empty.");
    
    public static Error InvalidDuration() => new("Service.InvalidDuration", "Invalid service duration. Duration must be 0 or 30 minutes.");
}