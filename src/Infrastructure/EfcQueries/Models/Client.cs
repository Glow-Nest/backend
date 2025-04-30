using System;
using System.Collections.Generic;

namespace EfcQueries.Models;

public partial class Client
{
    public Guid ClientId { get; set; }

    public bool IsVerified { get; set; }

    public string Email { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual OtpSession? OtpSession { get; set; }
}
