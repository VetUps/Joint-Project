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
    /// Логика взаимодействия для DishMoreInfoWindow.xaml
    /// </summary>
    public partial class DishMoreInfoWindow : Window
    {
        public DishMoreInfoWindow(Dish dish)
        {
            InitializeComponent();
            dishInfo = dish;

            GetAllergens();
        }

        Dish dishInfo { get; }

        private void GetAllergens()
        {
            string allergens_list = "";
            foreach(var allergen in dishInfo.Allergens)
            {
                allergens_list = allergens_list + ", " + allergen.AllergenName;
            }

            allergensTextBlock.Text = "Аллергены: " + allergens_list;
        }
    }
}
