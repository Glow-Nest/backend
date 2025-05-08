using System;
using System.Collections.Generic;

namespace EfcQueries.Models;

public partial class BlockedTime
{
    public Guid Id { get; set; }

    public string ScheduledDate { get; set; } = null!;

    public Guid? ScheduleId { get; set; }

    public string Reason { get; set; } = null!;

    public TimeOnly EndTime { get; set; }

    public TimeOnly StartTime { get; set; }

    public virtual Schedule? Schedule { get; set; }
}
