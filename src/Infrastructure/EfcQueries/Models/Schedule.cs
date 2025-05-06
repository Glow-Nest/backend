using System;
using System.Collections.Generic;

namespace EfcQueries.Models;

public partial class Schedule
{
    public Guid ScheduleId { get; set; }

    public string ScheduleDate { get; set; } = null!;

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual ICollection<BlockedTime> BlockedTimes { get; set; } = new List<BlockedTime>();
}
