using System;
using System.Collections.Generic;

namespace EfcQueries.Models;

public partial class ProductReview
{
    public Guid ProductReviewId { get; set; }

    public Guid ProductId { get; set; }

    public Guid ClientId { get; set; }

    public int Rating { get; set; }

    public string ReviewMessage { get; set; } = null!;

    public virtual Client Client { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
