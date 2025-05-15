using System;
using System.Collections.Generic;

namespace EfcQueries.Models;

public partial class ServiceReview
{
    public Guid ServiceReviewId { get; set; }

    public Guid ClientId { get; set; }

    public Guid ServiceId { get; set; }

    public int Rating { get; set; }

    public string ReviewMessage { get; set; } = null!;

    public virtual Client Client { get; set; } = null!;

    public virtual Service Service { get; set; } = null!;
}
