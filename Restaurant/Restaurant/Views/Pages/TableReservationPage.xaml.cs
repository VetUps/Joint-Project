using Restaurant.Models;
using Restaurant.Views.UserControls;
using Restaurant.Views.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class TableResebationPage : Page, INotifyPropertyChanged
    {
        public TableResebationPage()
        {
            InitializeComponent();
            DataContext = this;
            LoadTables();
            UpdatePage();

            dateChooseDateTimeUpDown.Minimum = DateTime.Now;
            dateChooseDateTimeUpDown.Maximum = DateTime.Now.AddMonths(1).AddHours(1);
        }

        private List<TableControl> tablesSource_ = new List<TableControl>();
        public List<TableControl> TableSource {  get { return tablesSource_; } }

        private List<Table> tablesSourceList_ = new List<Table>();
        public List<Table> TableSourceList { get { return tablesSourceList_; } }


        private List<string> possiblePages = new List<string>();
        private int currentPage_ = 0;
        private int maxPages_ = 0;

        public List<TableControl> CurrentPageItems { get; private set; }
        public string CurrentLocation { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void LoadTables()
        {
            using (var context = new RestaurantDbContext())
            {
                foreach (var table in context.Tables)
                {
                    tablesSource_.Add(new TableControl(table));
                    TableSourceList.Add(table);

                    if (!possiblePages.Contains(table.TableLocation))
                    {
                        possiblePages.Add(table.TableLocation);
                    }
                }
            }

            maxPages_ = possiblePages.Count - 1;
        }

        private void UpdatePage()
        {
            CurrentLocation = possiblePages[currentPage_];
            CurrentPageItems = TableSource.Where(o => o.Table.TableLocation == CurrentLocation).ToList();

            // Обновление привязок
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentPageItems)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentLocation)));
        }

        private void previousFrameButton_Click(object sender, RoutedEventArgs e)
        {
            currentPage_--;

            if (currentPage_ == -1)
            {
                currentPage_ = maxPages_;
            }

            UpdatePage();
        }

        private void nextFrameButton_Click(object sender, RoutedEventArgs e)
        {
            currentPage_++;

            if (currentPage_ > maxPages_)
            {
                currentPage_ = 0;
            }

            UpdatePage();
        }

        private void makeReservationButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void timeChooseButton_Click(object sender, RoutedEventArgs e)
        {
            new TimeReservationChooseWindow().ShowDialog();
        }
    }
}
