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
    /// Логика взаимодействия для BookingAdmin.xaml
    /// </summary>
    public partial class BookingAdmin : Page
    {
        private RestaurantDbContext dbContext = new RestaurantDbContext();
        public BookingAdmin()
        {
            InitializeComponent();
            Load();
        }
        private void Load()
        {

            var reservations = dbContext.Orders
                .Include(o => o.Client)
                .Select(o => new Reservation
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate.GetValueOrDefault(new DateOnly(1900, 1, 1)),
                    ClientName = o.Client.ClientName,
                    TableLocation = o.Client.ClientTables.FirstOrDefault().Table.TableLocation,
                    OrderStatus = o.OrderStatus, 
                   TableID = o.Client.ClientTables.FirstOrDefault().Table.TableId

                })
                .ToList();

            ReservationsListView.ItemsSource = reservations;
            
        }

        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.DataContext is Reservation selectedReservation)
            {
                try
                {
                    var orderToUpdate = dbContext.Orders.FirstOrDefault(o => o.OrderId == selectedReservation.OrderId);
                    if (orderToUpdate != null)
                    {
                        orderToUpdate.OrderStatus = selectedReservation.OrderStatus;
                        dbContext.SaveChanges();
                        MessageBox.Show("Статус заказа успешно обновлен!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка: {ex.Message}");
                }
            }
        }
        public class Reservation : INotifyPropertyChanged
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
            public string ReservationDetails => $"{OrderDate:yyyy-MM-dd} / Столик {TableID} / на имя: {ClientName} / местоположение: {TableLocation}";

            public ObservableCollection<string> StatusOptions { get; set; } = new ObservableCollection<string>
        {
            "Новый заказ",
            "Обработка заказа",
            "Готовится",
            "Подан"
        };

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    
    }
}
                  
