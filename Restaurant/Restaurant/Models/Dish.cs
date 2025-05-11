using System;
using System.Collections.Generic;

namespace Restaurant.Models;

public partial class Dish
{
    public int DishId { get; set; }

    public string? DishName { get; set; }

    public string? DishDescription { get; set; }

    public float? DishPrice { get; set; }

    public int? MenuCategoryId { get; set; }

    public string? DishImage { get; set; }

    public virtual MenuCategory? MenuCategory { get; set; }

    public virtual ICollection<Allergen> Allergens { get; set; } = new List<Allergen>();
    public string? GetImage
    {
        get
        {
            if (DishImage == null)
                return "pack://application:,,,/Resources/Images/dishImage.png";
            else
                return $"pack://application:,,,/Resources/Images/{DishImage}";
        }
    }
}
