using System;
using System.Collections.Generic;

namespace EfcQueries.Models;

public partial class Category
{
    public Guid CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public virtual ICollection<MediaUrl> MediaUrls { get; set; } = new List<MediaUrl>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
