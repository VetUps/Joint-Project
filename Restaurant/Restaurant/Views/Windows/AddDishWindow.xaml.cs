using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Restaurant.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddDishWindow.xaml
    /// </summary>
    public partial class AddDishWindow : Window
    {
        public AddDishWindow(Dish? dish)
        {
            InitializeComponent();

            _Dish = dish ?? new();
            if (dish != null)
                InitializingParameters();

            IsNewDish = dish == null;
            DataContext = _Dish;

            using (var context = new RestaurantDbContext())
            {
                dishCategoryComboBox.ItemsSource = context.MenuCategories.ToList();
                if (!IsNewDish)
                    dishCategoryComboBox.SelectedValue = _Dish.MenuCategoryId;

                allergens = context.Allergens.Select(a => a.AllergenName).Distinct().ToList();
            }

            allergenViewListBox.ItemsSource = allergens;
        }
        private Dish _Dish { get; set; }
        private bool IsNewDish { get; init; }

        private List<string?> allergens = new List<string?>();

        private void InitializingParameters()
        {
            titleTextBox.Text = "Редактировать позицию";

            if (_Dish.Allergens != null && _Dish.Allergens.Any())
            {
                foreach (var allergen in _Dish.Allergens)
                {
                    allergenViewListBox.SelectedItems.Add(allergen.AllergenName);
                }
            }
        }

        private void addImageButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Image files (*.png;*.jpg)|*.png;*.jpg|All files (*.*)|*.*",
                Title = "Выберите изображение"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                string fileName = Path.GetFileName(filePath);
                BitmapImage bitmapImage = new BitmapImage(new Uri(filePath));
                dishImage.Source = bitmapImage;
                _Dish.DishImage = fileName;
            }
        }

        private List<string> ValidationAddDish(Dish dish)
        {
            List<string> errors = new List<string>();

            if (dishCategoryComboBox.SelectedValue == null)
                errors.Add("Выберите к какой категории принадлежит блюдо");

            if (string.IsNullOrEmpty(dish.DishDescription))
                errors.Add("Заполните состав");

            if (string.IsNullOrEmpty(dish.DishPrice.ToString()))
                errors.Add("Введите цену");

            if (string.IsNullOrWhiteSpace(dishPriceTextBox.Text))
                errors.Add("Введите цену");

            if (dish.DishPrice <= 0)
                errors.Add("Введите корректную цену");

            if (string.IsNullOrEmpty(dish.DishName))
                errors.Add("Введите название блюда");

            return errors;
        }

        private void saveDishButton_Click(object sender, RoutedEventArgs e)
        {
            var errors = ValidationAddDish(_Dish);
            if (errors.Any())
            {
                MessageBox.Show($"Данные блюда некорректны:\n{string.Join('\n', errors)}", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var context = new RestaurantDbContext())
                {
                    var selectedAllergenNames = allergenViewListBox.SelectedItems.Cast<string>().ToList();

                    if (!IsNewDish)
                    {
                        var existingDish = context.Dishes.Include(d => d.Allergens)
                                                         .First(d => d.DishId == _Dish.DishId);

                        existingDish.DishName = _Dish.DishName;
                        existingDish.DishDescription = _Dish.DishDescription;
                        existingDish.DishPrice = _Dish.DishPrice;
                        existingDish.DishImage = _Dish.DishImage;
                        existingDish.MenuCategoryId = (int)dishCategoryComboBox.SelectedValue;

                        existingDish.Allergens.Clear();
                        foreach (var allergenName in selectedAllergenNames)
                        {
                            var allergen = context.Allergens.First(a => a.AllergenName == allergenName);
                            existingDish.Allergens.Add(allergen);
                        }

                        context.Dishes.Update(existingDish);
                    }
                    else
                    {
                        _Dish.MenuCategoryId = (int)dishCategoryComboBox.SelectedValue;
                        _Dish.Allergens = new List<Allergen>();

                        foreach (var allergenName in selectedAllergenNames)
                        {
                            var allergen = context.Allergens.First(a => a.AllergenName == allergenName);
                            _Dish.Allergens.Add(allergen);
                        }

                        context.Dishes.Add(_Dish);
                    }

                    context.SaveChanges();

                    DialogResult = true;
                    if (IsNewDish)
                        Close();

                    MessageBox.Show("Запись успешно сохранена!", 
                                    "Сохранение", 
                                    MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Сохранение данных прервано.\nПодробнее:\n{ex.InnerException?.Message ?? ex.Message}",
                                 "Ошибка!", 
                                 MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9]+");
            return !regex.IsMatch(text);
        }

        private void PriceDishesTb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }
    }
}
