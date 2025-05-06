using System;
using System.Collections.Generic;

namespace EfcQueries.Models;

public partial class SalonOwner
{
    public Guid SalonOwnerId { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
}
