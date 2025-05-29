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
                        ReservationCardSource.Add(new ReservationControl(reservation));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки: {ex.Message}");
            }
        }
    }
}
