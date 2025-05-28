using System;
using System.Collections.Generic;

namespace Restaurant.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? ClientId { get; set; }

    public DateOnly? OrderDate { get; set; }

    public string? OrderStatus { get; set; }

    public virtual Client? Client { get; set; }

    public virtual ICollection<DishOrder> DishOrders { get; set; } = new List<DishOrder>();
}
