using System;
using System.Collections.Generic;

namespace EfcQueries.Models;

public partial class Product
{
    public Guid ProductId { get; set; }

    public string Description { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;

    public int InventoryCount { get; set; }

    public double Price { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<ProductReview> ProductReviews { get; set; } = new List<ProductReview>();
}
