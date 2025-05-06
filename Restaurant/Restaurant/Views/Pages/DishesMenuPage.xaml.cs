using Microsoft.EntityFrameworkCore;
using Restaurant.Models;
using Restaurant.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Restaurant.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для DishesMenuPage.xaml
    /// </summary>
    public partial class DishesMenuPage : Page
    {
        public DishesMenuPage()
        {
            InitializeComponent();
            DataContext = this;
            LoadDishes();
        }

        // ObservableCollection заменит List, т.к. ListBox будет понимать, что в свойстве произошли изменения
        private ObservableCollection<DishCard> _dishCards = new();
        public ObservableCollection<DishCard> DishCardSource
        {
            get => _dishCards;
            set
            {
                _dishCards = value;
            }
        }

        private void LoadDishes()
        {
            try
            {
                using var db = new RestaurantDbContext();

                var dishes = db.Dishes.Include(o => o.MenuCategory)
                                      .Include(o => o.Allergens).AsNoTracking().ToList();

                DishCardSource.Clear();

                foreach (var dish in dishes)
                {
                    DishCardSource.Add(new DishCard(dish));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}");
            }
        }
    }
}
