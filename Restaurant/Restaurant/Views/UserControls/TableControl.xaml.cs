using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Restaurant.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для TableControl.xaml
    /// </summary>
    public partial class TableControl : UserControl
    {
        public TableControl(Table table)
        {
            InitializeComponent();
            DataContext = table;

            StatusColor();
        }

        private void StatusColor()
        {
            if (tableStatusTextBlock.Text == "Свободен")
                tableStatusTextBlock.Foreground = new SolidColorBrush(Colors.Green);

            else if (tableStatusTextBlock.Text == "Занят")
                tableStatusTextBlock.Foreground = new SolidColorBrush(Colors.Red);

            else if (tableStatusTextBlock.Text == "Зарезервирован")
                tableStatusTextBlock.Foreground = new SolidColorBrush(Colors.Yellow);
        }
    }
}
