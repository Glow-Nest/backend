using System;
using System.Collections.Generic;

namespace EfcQueries.Models;

public partial class Order
{
    public Guid OrderId { get; set; }

    public Guid ClientId { get; set; }

    public string PickupDate { get; set; } = null!;

    public string OrderDate { get; set; } = null!;

    public double TotalPrice { get; set; }

    public string OrderStatus { get; set; } = null!;

    public int PaymentStatus { get; set; }

    public virtual Client Client { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
