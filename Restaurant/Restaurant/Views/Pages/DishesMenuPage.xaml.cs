using Microsoft.EntityFrameworkCore;
using Restaurant.Models;
using Restaurant.Views.UserControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public partial class DishesMenuPage : Page, INotifyPropertyChanged
    {
        public DishesMenuPage()
        {
            InitializeComponent();
            DataContext = this;
            LoadDishes();
            LoadMenuCategories();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // ObservableCollection заменит List, т.к. ListBox будет понимать, что в свойстве произошли изменения
        private ObservableCollection<DishCard> _dishCards = new();
        public ObservableCollection<DishCard> DishCardSource
        {
            get => _dishCards;
            set
            {
                _dishCards = value;
                OnPropertyChanged();
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

        private void ApplyNavSettings()
        {
            LoadDishes();

            /*
            string searchText = searchTextBox.Text.ToLower();

            if (sortComboBox.SelectedIndex != -1)
            {
                string sortMethod = sortComboBox.SelectedItem.ToString();
                SortPartners(sortMethod);
            }
            */

            if (menuCategoryComboBox.SelectedIndex != -1)
            {
                string filterMethod = menuCategoryComboBox.SelectedItem.ToString();
                MenuCategoryFilter(filterMethod);
            }

            //SearchPartners(searchText);
        }

        /*
        private void SortPartners(string sortMethod)
        {
            ProductCardSource = ProductCardSource.OrderBy(o => o.Partner.CalculateDiscount).ToList();

            if (sortMethod.Last() == '↓')
            {
                // Что-то может быть
            }
            else if (sortMethod.Last() == '↑')
            {
                ProductCardSource.Reverse();
            }
        }

        private void SearchPartners(string serachField)
        {
            if (string.IsNullOrWhiteSpace(serachField) || string.IsNullOrEmpty(serachField))
            {
                // Может что-то быть
            }

            else
            {
                ProductCardSource = ProductCardSource.Where(o => o.Partner.PartnerImportName.ToLower().Contains(serachField)).ToList();
            }
        }
        */

        private void MenuCategoryFilter(string filterMethod)
        {
            if (filterMethod == "Без фильтрации")
            {
                // Возможно что-то
            }

            else
            {
                var filtered = _dishCards.Where(o => o.dishInfo.MenuCategory.MenuCategoryName == filterMethod).ToList();
                
                DishCardSource.Clear();
                foreach (var item in filtered)
                {
                    DishCardSource.Add(item);
                }
            }
        }
        private void LoadMenuCategories()
        {
            using var db = new RestaurantDbContext();

            var menuCategories = db.MenuCategories.ToList();

            foreach (var item in menuCategories)
            {
                menuCategoryComboBox.Items.Add(item.MenuCategoryName); 
            }
        }

        private void menuCategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyNavSettings();
        }
    }
}
