using System;
using System.Collections.Generic;

namespace Restaurant.Models;

public partial class MenuCategory
{
    public int MenuCategoryId { get; set; }

    public string? MenuCategoryName { get; set; }

    public virtual ICollection<Dish> Dishes { get; set; } = new List<Dish>();
}
