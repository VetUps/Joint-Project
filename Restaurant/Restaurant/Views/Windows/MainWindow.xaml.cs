using Restaurant.Views.Windows;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Restaurant
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Ставим MainMenuPage как страницу по умолчанию по запуску
            MainFrame.Navigate(new Uri("pack://application:,,,/Views/Pages/MainMenuPage.xaml"), UriKind.Relative);
        }

        // Событие на переход в меню блюд
        private void menuNavigateButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("pack://application:,,,/Views/Pages/DishesMenuPage.xaml"), UriKind.Relative);
        }


        private void reservationNavigateButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("pack://application:,,,/Views/Pages/TableReservationPage.xaml"), UriKind.Relative);
        }

        private void reviewsNavigationButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Uri("pack://application:,,,/Views/Pages/FeedbackClientPage.xaml"), UriKind.Relative);
        }

        private void adminEnterNavigationButton_Click(object sender, RoutedEventArgs e)
        {
            AuthorizationAdminWindow authorization = new AuthorizationAdminWindow();
            authorization.Owner = this;
            if (authorization.ShowDialog() == true)
                authorization.Close();

            else
                Close();
        }
    }
}