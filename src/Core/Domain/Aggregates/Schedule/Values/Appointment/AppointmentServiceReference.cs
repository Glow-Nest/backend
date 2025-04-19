using Domain.Aggregates.Service.Values;

namespace Domain.Aggregates.Appointment.Values;

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