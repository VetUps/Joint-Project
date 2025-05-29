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
using static Restaurant.Views.Pages.OrdersAdminPage;

namespace Restaurant.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdminReservationPage.xaml
    /// </summary>
    public partial class AdminReservationPage : Page, INotifyPropertyChanged
    {
        public AdminReservationPage()
        {
            InitializeComponent();

            DataContext = this;
            LoadReservations();
        }

        // Реализуем ивент для обновления списка броней
        public event PropertyChangedEventHandler? PropertyChanged;

        // Функция для сообщения об изменениях
        protected void OnPropertyChanged([CallerMemberName] string name = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        // ObservableCollection заменит List, т.к. ListBox будет понимать, что в свойстве произошли изменения
        private ObservableCollection<ReservationControl> _reservationCards = new();
        public ObservableCollection<ReservationControl> ReservationCardSource
        {
            get => _reservationCards;
            set
            {
                _reservationCards = value;
                OnPropertyChanged();
            }
        }

        private void LoadReservations()
        {
            try
            {
                using (var context = new RestaurantDbContext())
                {
                    var reservations = context.ClientTables
                        .Include(o => o.Client)
                        .Include(o => o.Table)
                    .ToList();

                    ReservationCardSource.Clear();

                    foreach (var reservation in reservations)
                    {
                        ReservationControl reservationControl = new ReservationControl(reservation);
                        reservationControl.deleteReservationButton.Click += deleteReservationButton_Click;
                        reservationControl.prolongReservationButton.Click += prolongReservationButton_Click;
                        reservationControl.reservationStatusComboBox.SelectionChanged += reservationStatusComboBox_SelectionChanged;

                        ReservationCardSource.Add(reservationControl);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}");
            }
        }

        private void deleteReservationButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            ClientTable clientTable = (button.DataContext as ReservationControl).ClientTableInfo;

            using (var context = new RestaurantDbContext())
            {
                context.ClientTables.Remove(clientTable);

                List<Models.Order> toDelete = context.Orders.Where(o => o.ClientId == clientTable.ClientId).ToList();
                foreach (var order in toDelete)
                {
                    context.Orders.Remove(order);
                }

                context.SaveChanges();
                MessageBox.Show("Бронь успешно удалена",
                                "Внимание!",
                                MessageBoxButton.OK, MessageBoxImage.Information);

                LoadReservations();
            }
        }

        private void prolongReservationButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            ClientTable clientTable = (button.DataContext as ReservationControl).ClientTableInfo;

            int selectedTableId = clientTable.TableId;
            int selectedClientId = clientTable.ClientId;

            DateTime selectedDate = clientTable.DatetimeFrom.Value.Date;
            TimeSpan selectedDateTimeFrom = clientTable.DatetimeFrom.Value.TimeOfDay;
            TimeSpan selectedDateTimeTo = clientTable.DatetimeTo.Value.TimeOfDay;

            using (var context = new RestaurantDbContext())
            {
                var anotherReservations = context.ClientTables.Where(o => o.TableId == selectedTableId && 
                                                                          o.DatetimeFrom.Value.Date == selectedDate
                                                                          && o.ClientId != selectedClientId).ToList();

                foreach (var otherReservations in anotherReservations)
                {
                    if (otherReservations.DatetimeFrom.Value.TimeOfDay == selectedDateTimeTo)
                    {
                        MessageBox.Show("Невозможно продлить бронь, так как на последующее время уже есть брони",
                                        "Внимание!",
                                        MessageBoxButton.OK, MessageBoxImage.Warning);

                        return;
                    }
                }

                var reservationToUpdate = context.ClientTables.FirstOrDefault(o => o.TableId == selectedTableId && o.ClientId == selectedClientId);

                var diff = reservationToUpdate.DatetimeTo.Value.AddMinutes(15) - reservationToUpdate.DatetimeFrom;
                reservationToUpdate.DatetimeTo = reservationToUpdate.DatetimeTo.Value.AddMinutes(15);

                context.SaveChanges();
                LoadReservations();

                MessageBox.Show("Бронь успешно продлена",
                                "Внимание!",
                                MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void reservationStatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && (comboBox.DataContext as ReservationControl).ClientTableInfo is ClientTable selectedReservation)
            {
                try
                {
                    using (var context = new RestaurantDbContext())
                    {
                        var reservationToUpdate = context.ClientTables.FirstOrDefault(o => o.ClientId == selectedReservation.ClientId && 
                                                                                           o.TableId == selectedReservation.TableId);
                        if (reservationToUpdate != null)
                        {
                            reservationToUpdate.ReservationStatus = selectedReservation.ReservationStatus;
                            context.SaveChanges();
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
}
