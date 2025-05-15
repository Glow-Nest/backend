using System;
using System.Collections.Generic;

namespace EfcQueries.Models;

public partial class OrderItem
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public double PriceWhenOrdering { get; set; }

    public Guid? OrderId { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product Product { get; set; } = null!;
}
