using Microsoft.EntityFrameworkCore;
using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Restaurant.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для TableReservationWindow.xaml
    /// </summary>
    public partial class TableReservationWindow : Window, INotifyPropertyChanged
    {
        public TableReservationWindow(Table selectedTable, DateTime selectedDate, 
                                      TimeSpan selectedTimeFrom, TimeSpan selectedTimeTo)
        {
            InitializeComponent();
            LoadDishes();

            DataContext = this;

            SelectedTable = selectedTable;
            SelectedDate = selectedDate;
            SelectedTimeFrom = selectedTimeFrom;
            SelectedTimeTo = selectedTimeTo;

            NotifyApp();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Table SelectedTable { get; set; }
        public DateTime SelectedDate { get; set; }
        public TimeSpan SelectedTimeFrom { get; set; }
        public TimeSpan SelectedTimeTo { get; set; }
        public List<Dish> SelectedDishesPreOrder {  get; set; } = new List<Dish>();
        public List<Dish> DishesSourceList {  get; set; } = new List<Dish>();
        public string UserData {  get; set; }

        private void NotifyApp()
        {
            UserData = "";
            UserData = $"Столик {SelectedTable.TableId} {SelectedTable.TableLocation}\n" +
                       $"{SelectedDate.ToShortDateString()} на {SelectedTimeFrom}-{SelectedTimeTo}\n" +
                       $"Позиции предзаказ:";

            foreach (var dish in SelectedDishesPreOrder)
            {
                UserData += $"\n{dish.DishName}";
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UserData"));
        }

        private void LoadDishes()
        {
            using (var context = new RestaurantDbContext())
            {
                DishesSourceList.Clear();
                DishesSourceList = context.Dishes.ToList();
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void dishesPreOrderCheckComboBox_ItemSelectionChanged(object sender, Xceed.Wpf.Toolkit.Primitives.ItemSelectionChangedEventArgs e)
        {
            SelectedDishesPreOrder.Clear();

            foreach (var dish in dishesPreOrderCheckComboBox.SelectedItems)
            {
                SelectedDishesPreOrder.Add(dish as Dish);
            }

            NotifyApp();
        }

        private bool IsValidEmail(string email)
        {
            // Простая валидация email
            return Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase);
        }

        private List<string> ValidateUserData()
        {
            var errors = new List<string>();

            // Проверка обязательных полей
            if (string.IsNullOrWhiteSpace(surnameWatermarkTextBox.Text))
                errors.Add("Фамилия обязательна для заполнения");

            if (string.IsNullOrWhiteSpace(nameWatermarkTextBox.Text))
                errors.Add("Имя обязательно для заполнения");

            if (string.IsNullOrWhiteSpace(phoneWatermarkTextBox.Text))
                errors.Add("Телефон обязателен для заполнения");

            // Проверка email (если введен)
            if (!string.IsNullOrWhiteSpace(emailWatermarkTextBox.Text)
                && !IsValidEmail(emailWatermarkTextBox.Text))
            {
                errors.Add("Некорректный формат email");
            }

            return errors;
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            var errors = ValidateUserData();

            if (errors.Any())
            {
                MessageBox.Show($"Возникла ошибка:\n{string.Join('\n', errors)}", 
                                 "Ошибка!", 
                                 MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var context = new RestaurantDbContext())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            // 1. Создаем клиента
                            Client newClient = new Client
                            {
                                ClientFirstname = surnameWatermarkTextBox.Text,
                                ClientName = nameWatermarkTextBox.Text,
                                ClientEmail = !string.IsNullOrWhiteSpace(emailWatermarkTextBox.Text)
                                             ? emailWatermarkTextBox.Text
                                             : null,
                                ClientPhone = phoneWatermarkTextBox.Text,
                            };

                            context.Clients.Add(newClient);
                            context.SaveChanges(); // Первое сохранение - генерирует ClientId

                            // 2. Создаем бронь столика
                            ClientTable newReservation = new ClientTable
                            {
                                ClientId = newClient.ClientId, // Теперь ClientId известен
                                TableId = SelectedTable.TableId,
                                DatetimeFrom = SelectedDate.Date + SelectedTimeFrom,
                                DatetimeTo = SelectedDate.Date + SelectedTimeTo,
                            };

                            context.ClientTables.Add(newReservation);

                            // 3. Обработка предзаказа
                            if (SelectedDishesPreOrder.Count != 0)
                            {
                                Order newOrder = new Order
                                {
                                    ClientId = newClient.ClientId,
                                    OrderDate = DateOnly.FromDateTime(SelectedDate),
                                    OrderStatus = "Предзаказ"
                                };

                                context.Orders.Add(newOrder);
                                context.SaveChanges(); // Генерирует OrderId

                                foreach (var dish in SelectedDishesPreOrder)
                                {
                                    DishOrder newDishOrder = new DishOrder
                                    {
                                        OrderId = newOrder.OrderId, // Теперь OrderId известен
                                        DishId = dish.DishId,
                                    };

                                    context.DishOrders.Add(newDishOrder);
                                }
                            }

                            // Финализируем изменения
                            context.SaveChanges();
                            transaction.Commit();

                            DialogResult = true;
                            Close();

                            MessageBox.Show("Бронь успешно создана!",
                                "Успешно!",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Бронирование прервано.\nПодробнее:\n{ex.InnerException?.Message ?? ex.Message}",
                    "Ошибка!",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
