using Restaurant.Models;
using Restaurant.Views.UserControls;
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

namespace Restaurant.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для TableResebationPage.xaml
    /// </summary>
    public partial class TableResebationPage : Page
    {
        public TableResebationPage()
        {
            InitializeComponent();
            LoadTables();
        }

        private List<TableControl> tablesSource_ = new List<TableControl>();
        public List<TableControl> TableSource {  get { return tablesSource_; } }

        private void LoadTables()
        {
            using (var context = new RestaurantDbContext())
            {
                foreach (var table in context.Tables)
                {
                    tablesSource_.Add(new TableControl(table));
                }
            }

            tablesListView.ItemsSource = TableSource;
        }
    }
}
