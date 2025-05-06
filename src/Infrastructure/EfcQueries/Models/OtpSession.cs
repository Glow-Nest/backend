using System;
using System.Collections.Generic;

namespace EfcQueries.Models;

public partial class OtpSession
{
    public string Email { get; set; } = null!;

    public Guid ClientId { get; set; }

    public string OtpCode { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public int Purpose { get; set; }

    public bool IsUsed { get; set; }

    public virtual Client Client { get; set; } = null!;
}
