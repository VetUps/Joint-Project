using Restaurant.Models;
using Restaurant.Views.Pages;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Restaurant.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow(Admin admin_)
        {
            InitializeComponent();
            // Ставим MainMenuPage как страницу по умолчанию по запуску
            MainFrame.Navigate(new Uri("pack://application:,,,/Views/Pages/MainMenuPage.xaml"), UriKind.Relative);

            Admin = admin_;
        }

        private Admin Admin;

        private void ordersNavigationButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("pack://application:,,,/Views/Pages/OrdersAdminPage.xaml"), UriKind.Relative);
        }

        private void tablesNavigationButton_Click(object sender, RoutedEventArgs e)
        {
            // Переход на фрейм для управления столиками
        }

        private void reservationsNavigationButton_Click(object sender, RoutedEventArgs e)
        {
            // Переход на фрейм для управления бронями
        }

        private void dishMenuNavigationButton_Click(object sender, RoutedEventArgs e)
        {
            // Переход на фрейм для редактирования меню
        }

        private void reviewsNavigationButton_Click(object sender, RoutedEventArgs e)
        {
            // Переход на фрейм для модерации отзывов
        }

        private void reportForDishesNavigationButton_Click(object sender, RoutedEventArgs e)
        {
            // Переход на фрейм для просмотра отчётности
        }

        private void adminExitNavigationButton_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }
    }
}
