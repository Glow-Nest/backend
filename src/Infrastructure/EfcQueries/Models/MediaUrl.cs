using System;
using System.Collections.Generic;

namespace EfcQueries.Models;

public partial class MediaUrl
{
    public Guid Id { get; set; }

    public string Url { get; set; } = null!;

    public Guid CategoryId { get; set; }

    public virtual Category Category { get; set; } = null!;
}
