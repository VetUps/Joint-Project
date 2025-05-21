using Microsoft.EntityFrameworkCore;
using Restaurant.Models;
using Restaurant.Views.UserControls;
using Restaurant.Views.Windows;
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
using Xceed.Wpf.AvalonDock.Controls;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

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

            // Ставим DataContext, чтобы все Binding-и работали исправно
            DataContext = this;
            // Загружаем блюда
            LoadDishes();
            // Загружаем фильтр по категории меню
            LoadMenuCategories();
        }

        // Реализуем ивент для обновления списка блюд
        public event PropertyChangedEventHandler? PropertyChanged;

        // Функция для сообщения об изменениях
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

        // Функция загрузки блюд
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

        // Функция для применения всех выбранных фильтров
        private void ApplyNavSettings()
        {
            LoadDishes();

            List<string> filters = menuCategoryComboBox.SelectedItems.Cast<string>().ToList();
            MenuCategoryFilter(filters);

            List<string> exceptions = exceptionMenuCategoryCheckComboBox.SelectedItems.Cast<string>().ToList();
            ExceptionMenuCategoryFilter(exceptions);

            SearchPartners(searchWatermarkTextBox.Text);

            GetRangeValues(out int? from, out int? to);
            PriceFilter(from, to);
        }

        // Функция фильтрации по категориям меню
        private void MenuCategoryFilter(List<string> filters)
        {
            if (filters.Count == 0)
                return;

            var filtered = _dishCards.Where(o => !filters.Contains(o.dishInfo.MenuCategory.MenuCategoryName)).ToList();
                
            foreach (var item in filtered)
            {
                DishCardSource.Remove(item);
            }
        }

        // Функция для заполнения фильтра меню категорий
        private void LoadMenuCategories()
        {
            using var db = new RestaurantDbContext();

            var menuCategories = db.MenuCategories.ToList();

            foreach (var item in menuCategories)
            {
                menuCategoryComboBox.Items.Add(item.MenuCategoryName);
                exceptionMenuCategoryCheckComboBox.Items.Add(item.MenuCategoryName);
            }
        }

        // Событие на выбор фильтра категории меню
        private void menuCategoryComboBox_ItemSelectionChanged(object sender, Xceed.Wpf.Toolkit.Primitives.ItemSelectionChangedEventArgs e)
        {
            ApplyNavSettings();
        }

        // Функция фильтрации по категориям меню
        private void ExceptionMenuCategoryFilter(List<string> exceptions)
        {
            var filtered = _dishCards.Where(o => exceptions.Contains(o.dishInfo.MenuCategory.MenuCategoryName)).ToList();

            foreach (var item in filtered)
            {
                DishCardSource.Remove(item);
            }
        }

        private void exceptionMenuCategoryCheckComboBox_ItemSelectionChanged(object sender, Xceed.Wpf.Toolkit.Primitives.ItemSelectionChangedEventArgs e)
        {
            ApplyNavSettings();
        }

        private void SearchPartners(string searchField)
        {
            if (string.IsNullOrWhiteSpace(searchField) || string.IsNullOrEmpty(searchField))
            {
                // Может что-то быть
            }

            else
            {
                var filtered = _dishCards.Where(o => !o.dishInfo.DishName.ToLower().Contains(searchField.ToLower())).ToList();

                foreach (var item in filtered)
                {
                    DishCardSource.Remove(item);
                }
            }
        }
        private void searchWatermarkTextBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            ApplyNavSettings();
        }

        private void PriceFilter(int? from, int? to)
        {
            var filtered = _dishCards.Where(o => !(o.dishInfo.DishPrice >= from && o.dishInfo.DishPrice <= to)).ToList();

            foreach (var item in filtered)
            {
                DishCardSource.Remove(item);
            }
        }

        private void GetRangeValues(out int? from, out int? to)
        {
            from = 0;
            to = 9999;

            // Получаем текст без символов маски
            string rawText = priceRangeMaskedTextBox.Text
                .Replace("от", "")
                .Replace("до", "")
                .Trim();

            string[] parts = rawText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length >= 2)
            {
                if (int.TryParse(parts[0].Replace("_", ""), out int fromValue))
                {
                    from = fromValue;
                }

                if (int.TryParse(parts[1].Replace("_", ""), out int toValue))
                {
                    to = toValue;
                }
            }
        }

        private void priceRangeMaskedTextBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            ApplyNavSettings();
        }

        private void dishesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DishCard dishCard = (DishCard)dishesListBox.SelectedItem;
            new DishMoreInfoWindow(dishCard.dishInfo).ShowDialog();
        }
    }
}
