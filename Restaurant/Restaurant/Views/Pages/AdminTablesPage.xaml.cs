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
    /// Логика взаимодействия для AdminTablesPage.xaml
    /// </summary>
    public partial class AdminTablesPage : Page
    {
        public AdminTablesPage()
        {
            InitializeComponent();
            LoadTables();
        }

        private void LoadTables()
        {
            using (var context = new RestaurantDbContext())
            {
                var tables = context.Tables
                .Select(t => new TableViewModel
                {
                    TableId = t.TableId,
                    TableLocation = t.TableLocation,
                    TableStatus = t.TableStatus
                })
                .ToList();

                tablesListView.ItemsSource = tables;
            }
        }

        private void statusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.DataContext is TableViewModel selectedTable)
            {
                try
                {
                    using (var context = new RestaurantDbContext())
                    {
                        var tableToUpdate = context.Tables.FirstOrDefault(t => t.TableId == selectedTable.TableId);
                        if (tableToUpdate != null)
                        {
                            tableToUpdate.TableStatus = selectedTable.TableStatus;
                            context.SaveChanges();

                            MessageBox.Show("Статус столика успешно обновлен!", 
                                            "Успех",
                                            MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при обновлении статуса: {ex.Message}",
                                     "Ошибка", 
                                     MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public class TableViewModel : INotifyPropertyChanged
        {
            public int TableId { get; set; }
            public int TableCapacity { get; set; }
            public string TableLocation { get; set; }

            private string _tableStatus;
            public string TableStatus
            {
                get => _tableStatus;
                set
                {
                    if (_tableStatus != value)
                    {
                        _tableStatus = value;
                        OnPropertyChanged(nameof(TableStatus));
                    }
                }
            }

            public ObservableCollection<string> StatusOptions { get; } = new ObservableCollection<string>
            {
                "Свободен",
                "Занят",
                "Забронирован"
            };
            public string ReservationDetails => $"Столик {TableId} / местоположение: {TableLocation}";

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
