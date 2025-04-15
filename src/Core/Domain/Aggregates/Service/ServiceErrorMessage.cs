using Domain.Common.OperationResult;

namespace Domain.Aggregates.Service;

public class ServiceErrorMessage
{
    public static Error ServiceNotFound() => new("Service.ServiceNotFound", "Service not found.");
    public static Error NoServicesSelected() => new("Service.NoServiceSelected", "No service selected.");

}