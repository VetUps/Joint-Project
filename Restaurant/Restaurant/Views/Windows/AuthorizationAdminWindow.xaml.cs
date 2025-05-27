using Restaurant.Models;
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
    /// Логика взаимодействия для AuthorizationAdminWindow.xaml
    /// </summary>
    public partial class AuthorizationAdminWindow : Window
    {
        public AuthorizationAdminWindow()
        {
            InitializeComponent();
        }

        private string CurPassword()
        {
            if (adminPasswordPasswordBox.Visibility == Visibility.Visible)
                return adminPasswordPasswordBox.Password;
            return adminPasswordTextBox.Text;

        }

        private void showPasswordCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            adminPasswordPasswordBox.Visibility = Visibility.Hidden;
            adminPasswordTextBox.Visibility = Visibility.Visible;
            adminPasswordTextBox.Text = adminPasswordPasswordBox.Password;
        }

        private void showPasswordCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            adminPasswordPasswordBox.Visibility = Visibility.Visible;
            adminPasswordTextBox.Visibility = Visibility.Hidden;
            adminPasswordPasswordBox.Password = adminPasswordTextBox.Text;
        }

        private List<string> ValidationAdmin()
        {
            List<string> errors = new List<string>();
            if (string.IsNullOrEmpty(LoginTb.Text))
                errors.Add("Введите логин");

            if (string.IsNullOrEmpty(CurPassword()))
                errors.Add("Введите пароль");
            return errors;
        }

        private void enterButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> errors = ValidationAdmin();
            if (errors.Any())
            {
                MessageBox.Show($"Некорректные данные!\nПодробнее:\n{string.Join('\n', errors)}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using (var context = new RestaurantDbContext())
                {
                    if (context.Admins.FirstOrDefault(a => a.AdminPassword == CurPassword() && a.AdminLogin == LoginTb.Text) is Admin admin)
                    {
                        DialogResult = false;
                        MessageBox.Show("Открытие окна админа");
                    }
                    else
                        MessageBox.Show("Ваша учётная запись не была найдена.", "Предупреждение!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при попытке входа в систему.\nПодробнее:\n{ex.Message}", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
