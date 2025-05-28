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

namespace Restaurant.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdminDishesMenuPage.xaml
    /// </summary>
    public partial class AdminDishesMenuPage : Page, INotifyPropertyChanged
    {
        public AdminDishesMenuPage()
        {
            InitializeComponent();

            // Ставим DataContext, чтобы все Binding-и работали исправно
            DataContext = this;
            // Загружаем блюда
            LoadDishes();
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
                using (var context = new RestaurantDbContext())
                {
                    var dishes = context.Dishes.Include(o => o.MenuCategory)
                                               .Include(o => o.Allergens).AsNoTracking().ToList();

                    DishCardSource.Clear();

                    foreach (var dish in dishes)
                    {
                        DishCardSource.Add(new DishCard(dish));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при попытке загрузке данных об отзывах.\nПодробнее:\n{ex.Message}", 
                                 "Ошибка!",
                                 MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void addDishButton_Click(object sender, RoutedEventArgs e)
        {
            AddDishWindow productWindow = new AddDishWindow(null);
            bool result = productWindow?.ShowDialog() ?? false;

            if (result)
                LoadDishes();
        }

        private void editDishButton_Click(object sender, RoutedEventArgs e)
        {
            if (dishesListBox.SelectedItem is DishCard card)
            {
                AddDishWindow productWindow = new AddDishWindow(card.dishInfo);
                bool result = productWindow?.ShowDialog() ?? false;
                if (result)
                {
                    //RestaurantDbContext.Instance.Entry(card._Dish).State = EntityState.Modified;
                    LoadDishes();
                }
            }
        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            var mesDelete = MessageBox.Show("Удаление необратимо.\nВы уверены?", 
                                            "Внимание!", 
                                            MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (mesDelete == MessageBoxResult.No)
                return;

            try
            {
                using (var context = new RestaurantDbContext())
                {
                    if (dishesListBox.SelectedItem is DishCard card)
                    {
                        var dishToRemove = context.Dishes.Find(card.dishInfo.DishId);
                        if (dishToRemove == null)
                            return;
                        dishToRemove.Allergens.Clear();
                        context.Dishes.Remove(dishToRemove);
                        context.SaveChanges();

                        LoadDishes();
                        MessageBox.Show("Блюдо успешно удалено", 
                                        "Успех", 
                                        MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                        MessageBox.Show("Для удаления выберите элемент из списка.", 
                                        "Внимание", 
                                        MessageBoxButton.OK, MessageBoxImage.Warning);

                    context.SaveChanges();
                }
            }
            catch
            {
                MessageBox.Show("Произошла ошибка при удалении элемента", 
                                "Ошибка!", 
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
