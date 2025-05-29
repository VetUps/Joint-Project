using Microsoft.EntityFrameworkCore;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Classes
{
    public class RestaurantAnalyticsService
    {
        private readonly RestaurantDbContext _context;
        public RestaurantAnalyticsService(RestaurantDbContext context)
        {
            _context = context;
        }

        public List<PopularDish> GetPopularDishesByCategory()
        {
            return _context.DishOrders
                .Include(d => d.Dish)
                .ThenInclude(d => d.MenuCategory)
                .GroupBy(d => new {
                    d.Dish.MenuCategory.MenuCategoryId,
                    d.Dish.MenuCategory.MenuCategoryName,
                    d.Dish.DishName,
                    d.Dish.DishPrice
                })
                .Select(g => new PopularDish
                {
                    CategoryId = g.Key.MenuCategoryId,
                    CategoryName = g.Key.MenuCategoryName,
                    DishName = g.Key.DishName,
                    OrderCount = g.Count(),
                    DishPrice = g.Key.DishPrice
                })
                .OrderBy(pd => pd.CategoryName)
                .ThenByDescending(pd => pd.OrderCount)
                .ToList();
        }

        public List<Category> GetCategories()
        {
            return _context.MenuCategories
                .Select(mc => new Category
                {
                    Id = mc.MenuCategoryId,
                    Name = mc.MenuCategoryName
                })
                .ToList();
        }


        public RestaurantRatingSummary GetRestaurantRating()
        {
            var summary = _context.Feedbacks
                .GroupBy(f => 1)
                .Select(g => new RestaurantRatingSummary
                {
                    AverageRating = g.Average(f => f.Rating),
                    TotalFeedbacks = g.Count(),
                    FiveStarCount = g.Count(f => f.Rating == 5),
                    FourStarCount = g.Count(f => f.Rating == 4),
                    ThreeStarCount = g.Count(f => f.Rating == 3),
                    TwoStarCount = g.Count(f => f.Rating == 2),
                    OneStarCount = g.Count(f => f.Rating == 1)
                })
                .FirstOrDefault();

            return summary;
        }
    }


    public class PopularDish
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string DishName { get; set; }
        public int OrderCount { get; set; }
        public float? DishPrice { get; set; }
    }


    public class RestaurantRatingSummary
    {
        public double? AverageRating { get; set; }
        public int TotalFeedbacks { get; set; }
        public int FiveStarCount { get; set; }
        public int FourStarCount { get; set; }
        public int ThreeStarCount { get; set; }
        public int TwoStarCount { get; set; }
        public int OneStarCount { get; set; }

        public double FiveStarPercentage => TotalFeedbacks > 0 ? (double)FiveStarCount / TotalFeedbacks * 100 : 0;
        public double FourStarPercentage => TotalFeedbacks > 0 ? (double)FourStarCount / TotalFeedbacks * 100 : 0;
        public double ThreeStarPercentage => TotalFeedbacks > 0 ? (double)ThreeStarCount / TotalFeedbacks * 100 : 0;
        public double TwoStarPercentage => TotalFeedbacks > 0 ? (double)TwoStarCount / TotalFeedbacks * 100 : 0;
        public double OneStarPercentage => TotalFeedbacks > 0 ? (double)OneStarCount / TotalFeedbacks * 100 : 0;
    }
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}