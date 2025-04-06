using System;
using System.Collections.Generic;

namespace Restaurant.Models;

public partial class Allergen
{
    public int AllergenId { get; set; }

    public string? AllergenName { get; set; }

    public virtual ICollection<Dish> Dishes { get; set; } = new List<Dish>();
}
