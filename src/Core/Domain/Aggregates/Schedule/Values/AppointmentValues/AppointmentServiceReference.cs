using Domain.Aggregates.ServiceCategory.Values;

namespace Domain.Aggregates.Schedule.Values.AppointmentValues;

public class AppointmentServiceReference
{
    internal ServiceId ServiceId { get; }
    
    public static implicit operator ServiceId(AppointmentServiceReference reference)
        => reference.ServiceId;

    public static implicit operator AppointmentServiceReference(ServiceId serviceIds)
        => new(serviceIds);
    
    public AppointmentServiceReference(ServiceId serviceIds)
    {
        ServiceId = serviceIds;
    }
    
    private AppointmentServiceReference() { }
}