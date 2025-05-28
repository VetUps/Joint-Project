using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Restaurant.Models;
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
        public TimeReservationChooseWindow(DateTime choosenDate)
        {
            InitializeComponent();

            DataContext = this;
            ChoosenDate = choosenDate;

            FillTimes();
            BlockTimes();
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

        DateTime ChoosenDate { get; set; }
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
                TimeControl timeControl = new TimeControl(times[timeIndex], times[timeIndex + 1]);

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

        private void BlockTimes()
        {
            using (var context = new RestaurantDbContext())
            {
                var reservations = context.ClientTables.Where(o => o.DatetimeFrom.Value.Date == ChoosenDate).ToList();

                foreach (var timeCard in TimeCardsSource)
                {
                    foreach (var reservation in reservations)
                    {
                        if (timeCard.TimeFrom == reservation.DatetimeFrom.Value.TimeOfDay)
                            DisableRange(timeCard.TimeFrom, timeCard.TimeTo);
                    }
                }
            }
        }

        private List<TimeSpan> GenerateTimeSlots(TimeSpan timeFrom, TimeSpan timeTo)
        {
            var timeSlots = new List<TimeSpan>();
            TimeSpan current = timeFrom;

            while (current <= timeTo.Add(TimeSpan.FromMinutes(15)))
            {
                timeSlots.Add(current);
                current = current.Add(TimeSpan.FromMinutes(15));
            }

            return timeSlots;
        }

        private void DisableRange(TimeSpan timeFrom, TimeSpan timeTo)
        {
            List <TimeSpan> blockedTime = GenerateTimeSlots(timeFrom, timeTo);

            foreach (var timeCard in TimeCardsSource)
            {
                if (blockedTime.Contains(timeCard.TimeFrom))
                    timeCard.chooseTimeCheckBox.IsEnabled = false;
            }
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

                try
                {
                    CurrentItems = TimeCardsSource.GetRange((int)firstSelectedTime, 2);
                }
                catch
                {
                    CurrentItems = TimeCardsSource.GetRange((int)firstSelectedTime, 1);
                }
            }

            else
            {
                CurrentItems.Where(o => o.ControlIndex == timeControl.ControlIndex - 1).ToList()[0].chooseTimeCheckBox.IsEnabled = false;

                if (selectedItemsCount == 8)
                {
                    return;
                }

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
