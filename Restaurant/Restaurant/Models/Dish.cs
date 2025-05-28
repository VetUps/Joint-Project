using Microsoft.Win32;
using Restaurant.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Policy;
using System.Windows.Media.Imaging;

namespace Restaurant.Models;

public partial class Dish
{
    public int DishId { get; set; }

    public string? DishName { get; set; }

    public string? DishDescription { get; set; }

    public float? DishPrice { get; set; }

    public int? MenuCategoryId { get; set; }

    public byte[]? DishImage { get; set; }

    public virtual ICollection<DishOrder> DishOrders { get; set; } = new List<DishOrder>();

    public virtual MenuCategory? MenuCategory { get; set; }

    public virtual ICollection<Allergen> Allergens { get; set; } = new List<Allergen>();
    public BitmapImage GetImage
    {
        get
        {
            if (DishImage == null)
                return new BitmapImage(new Uri("pack://application:,,,/Resources/Images/dishImage.png"));
            else
                return ImageConverter.LoadImageFromBytes(DishImage);
        }
    }
}
