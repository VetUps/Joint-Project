using System;
using System.Collections.Generic;

namespace Restaurant.Models;

public partial class DishOrder
{
    public int OrderId { get; set; }

    public int DishId { get; set; }

    public virtual Dish Dish { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
