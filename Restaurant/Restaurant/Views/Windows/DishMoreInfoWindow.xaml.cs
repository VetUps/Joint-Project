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
            List<string> allergens_list = new List<string>();

            foreach(var allergen in dishInfo.Allergens)
            {
                allergens_list.Add(allergen.AllergenName);
            }

            if (allergens_list.Count > 0)
            {
                allergensTextBlock.Text = "Аллергены: " + string.Join(", ", allergens_list);
            }

            else
            {
                allergensTextBlock.Text = "Для данного блюда пока не добавили аллергенов либо их нет";
            }
        }
    }
}
