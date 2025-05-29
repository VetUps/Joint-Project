using Microsoft.EntityFrameworkCore;
using Restaurant.Models;
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
    /// Логика взаимодействия для OrdersAdminPage.xaml
    /// </summary>
    public partial class OrdersAdminPage : Page
    {
        public OrdersAdminPage()
        {
            InitializeComponent();
            Load();
        }

        public class Order : INotifyPropertyChanged
        {
            public int OrderId { get; set; }
            public DateOnly OrderDate { get; set; }
            public string ClientName { get; set; }
            public string TableLocation { get; set; }
            public int TableID { get; set; }

            private string _orderStatus;
            public string OrderStatus
            {
                get { return _orderStatus; }
                set
                {
                    if (_orderStatus != value)
                    {
                        _orderStatus = value;
                        OnPropertyChanged(nameof(OrderStatus));
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            public string OrderDetails => $"{OrderDate:yyyy-MM-dd} / Столик {TableID} / на имя: {ClientName} / местоположение: {TableLocation}";

            public ObservableCollection<string> StatusOptions { get; set; } = new ObservableCollection<string>
            {
                "Предзаказ",
                "Новый заказ",
                "Обработка заказа",
                "Готовится",
                "Подан"
            };
        }
        
        private void Load()
        {
            using (var context = new RestaurantDbContext())
            {
                var orders = context.Orders
                .Include(o => o.Client)
                .ThenInclude(o => o.ClientTables)
                .ThenInclude(o => o.Table)
                .Select(o => new Order
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate.GetValueOrDefault(new DateOnly(1900, 1, 1)),
                    ClientName = o.Client.ClientName,
                    TableLocation = o.Client.ClientTables.FirstOrDefault().Table.TableLocation,
                    OrderStatus = o.OrderStatus,
                    TableID = o.Client.ClientTables.FirstOrDefault().Table.TableId

                })
                .ToList();

                ordersListView.ItemsSource = orders;
            }

        }

        private void statusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.DataContext is Order selectedOrder)
            {
                try
                {
                    using (var context = new RestaurantDbContext())
                    {
                        var orderToUpdate = context.Orders.FirstOrDefault(o => o.OrderId == selectedOrder.OrderId);
                        if (orderToUpdate != null)
                        {
                            orderToUpdate.OrderStatus = selectedOrder.OrderStatus;
                            context.SaveChanges();
                            MessageBox.Show("Статус заказа успешно обновлен!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
        }


        private void deleteOrderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Order selectedOrder = (sender as Button).DataContext as Order;

                using (var context = new RestaurantDbContext())
                {
                    var orderToDelete = context.Orders
                        .Include(o => o.DishOrders) // Подгружаем связанные DishOrders
                        .FirstOrDefault(o => o.OrderId == selectedOrder.OrderId);

                    if (orderToDelete != null)
                    {
                        context.DishOrders.RemoveRange(orderToDelete.DishOrders); // Удаляем все DishOrders
                        context.Orders.Remove(orderToDelete);
                        context.SaveChanges();

                        Load();
                        MessageBox.Show("Заказ успешно удалён!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}
