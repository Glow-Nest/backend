using Domain.Aggregates.ServiceCategory.Values;

namespace Domain.Aggregates.Schedule.Values.AppointmentValues;

public class AppointmentServiceReference
{
    internal ServiceId ServiceId { get; }
    
    public AppointmentServiceReference(ServiceId serviceIds)
    {
        ServiceId = serviceIds;
    }
    
    private AppointmentServiceReference() { }
}