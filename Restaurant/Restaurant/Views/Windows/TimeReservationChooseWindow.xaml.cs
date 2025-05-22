using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Restaurant.Views.UserControls;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Restaurant.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для TimeReservationChooseWindow.xaml
    /// </summary>
    public partial class TimeReservationChooseWindow : Window, INotifyPropertyChanged
    {
        public TimeReservationChooseWindow()
        {
            InitializeComponent();
            FillTimes();

            DataContext = this;
        }

        private List<TimeControl> _timeCards = new();
        public List<TimeControl> TimeCardsSource
        {
            get => _timeCards;
            set
            {
                _timeCards = value;
            }
        }

        int? firstSelectedTime = null;
        int? lastSelectedTime = null;
        int selectedItemsCount = 0;
        public List<TimeControl> CurrentItems { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void FillTimes()
        {
            List<TimeSpan> times = new List<TimeSpan>();

            TimeSpan startTime = TimeSpan.FromHours(8); // 08:00
            TimeSpan endTime = TimeSpan.FromHours(23);   // 23:00
            TimeSpan interval = TimeSpan.FromMinutes(15);

            for (TimeSpan current = startTime; current <= endTime; current += interval)
            {
                times.Add(current);
            }

            for (int timeIndex = 0; timeIndex < times.Count - 1; timeIndex++)
            {
                TimeControl timeControl = new TimeControl();

                timeControl.timeFromTextBlock.Text = $"{times[timeIndex]}";
                timeControl.timeToTextBlock.Text = $"{times[timeIndex + 1]}";

                timeControl.chooseTimeCheckBox.Checked += CheckBox_Checked;
                timeControl.chooseTimeCheckBox.Unchecked += CheckBox_Unchecked;

                timeControl.ControlIndex = timeIndex;

                TimeCardsSource.Add(timeControl);
            }

            TimeCardsSource.Last().ControlIndex = TimeCardsSource.Count - 1;

            CurrentItems = TimeCardsSource;
        }

        private void NotifyApp()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentItems"));
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            selectedItemsCount++;

            CheckBox checkBox = sender as CheckBox;
            TimeControl timeControl = checkBox.DataContext as TimeControl;

            if (firstSelectedTime == null)
            {
                firstSelectedTime = timeControl.ControlIndex;
                CurrentItems = TimeCardsSource.GetRange((int)firstSelectedTime, 2);
            }

            else
            {
                CurrentItems.Where(o => o.ControlIndex == timeControl.ControlIndex - 1).ToList()[0].chooseTimeCheckBox.IsEnabled = false;

                try
                {
                    CurrentItems = TimeCardsSource.GetRange((int)firstSelectedTime, selectedItemsCount + 1);
                }
                catch { }
            }

            lastSelectedTime = timeControl.ControlIndex;
            NotifyApp();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            selectedItemsCount--;

            CheckBox checkBox = sender as CheckBox;
            TimeControl timeControl = checkBox.DataContext as TimeControl;

            if (selectedItemsCount == 0)
            {
                firstSelectedTime = null;
                CurrentItems = TimeCardsSource;
            }

            else
            {
                CurrentItems.Where(o => o.ControlIndex == timeControl.ControlIndex - 1).ToList()[0].chooseTimeCheckBox.IsEnabled = true;
                CurrentItems = TimeCardsSource.GetRange((int)firstSelectedTime, selectedItemsCount + 1);
            }

            lastSelectedTime = timeControl.ControlIndex;
            NotifyApp();
        }

        private void timesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageBox.Show("fdsfsdf");
        }
    }
}
