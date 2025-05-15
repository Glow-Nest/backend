using System;
using System.Collections.Generic;

namespace EfcQueries.Models;

public partial class Service
{
    public Guid ServiceId { get; set; }

    public double Duration { get; set; }

    public string Name { get; set; } = null!;

    public double Price { get; set; }

    public Guid? CategoryId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<ServiceReview> ServiceReviews { get; set; } = new List<ServiceReview>();

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
