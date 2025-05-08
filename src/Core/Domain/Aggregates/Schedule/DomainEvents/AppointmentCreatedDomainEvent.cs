using Domain.Aggregates.Client.Values;
using Domain.Aggregates.Schedule.Values;
using Domain.Common;

namespace Domain.Aggregates.Schedule.DomainEvents;

public class AppointmentCreatedDomainEvent : IDomainEvent
{
    public ClientId ClientId { get; private set; }
    public DateOnly AppointmentDate { get; private set; }
    public TimeSlot TimeSlot { get; private set; }
    
    protected AppointmentCreatedDomainEvent(ClientId clientId, DateOnly appointmentDate, TimeSlot timeSlot)
    {
        ClientId = clientId;
        AppointmentDate = appointmentDate;
        TimeSlot = timeSlot;
    }
    
    public static AppointmentCreatedDomainEvent Create(ClientId clientId, DateOnly appointmentDate, TimeSlot timeSlot)
    {
        return new AppointmentCreatedDomainEvent(clientId, appointmentDate, timeSlot);
    }
}