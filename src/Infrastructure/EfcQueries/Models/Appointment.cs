using System;
using System.Collections.Generic;

namespace EfcQueries.Models;

public partial class Appointment
{
    public Guid Id { get; set; }

    public int AppointmentStatus { get; set; }

    public string AppointmentDate { get; set; } = null!;

    public Guid BookedByClient { get; set; }

    public Guid? ScheduleId { get; set; }

    public string Note { get; set; } = null!;

    public TimeOnly EndTime { get; set; }

    public TimeOnly StartTime { get; set; }

    public virtual Client BookedByClientNavigation { get; set; } = null!;

    public virtual Schedule? Schedule { get; set; }

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
