﻿using Restaurant.Models;
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
            UpdateData();

            dateChooseDateTimeUpDown.Minimum = DateTime.Now;
            dateChooseDateTimeUpDown.Maximum = DateTime.Now.AddMonths(1).AddHours(1);
        }
        public Table SelectedTable { get; set; }
        public DateTime SelectedDate { get; set; }
        public TimeSpan TimeFrom {  get; set; }
        public TimeSpan TimeTo { get; set; }

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

        private void UpdateData()
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

            UpdateData();
        }

        private void nextFrameButton_Click(object sender, RoutedEventArgs e)
        {
            currentPage_++;

            if (currentPage_ > maxPages_)
            {
                currentPage_ = 0;
            }

            UpdateData();
        }

        private void makeReservationButton_Click(object sender, RoutedEventArgs e)
        {
            new TableReservationWindow(SelectedTable, SelectedDate, TimeFrom, TimeTo).ShowDialog();
        }

        private void timeChooseButton_Click(object sender, RoutedEventArgs e)
        {
            if (dateChooseDateTimeUpDown.Value == null)
            {
                MessageBox.Show($"Вы не выбрали дату броинрования",
                                 "Ошибка!",
                                 MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            DateTime choosenDateTime = (DateTime)dateChooseDateTimeUpDown.Value;
            DateTime choosenDate = choosenDateTime.Date;

            Table choosenTable = (Table)locationChooseComboBox.SelectedItem;

            TimeReservationChooseWindow timeChooseWindow = new TimeReservationChooseWindow(choosenDate, choosenTable);
            timeChooseWindow.ShowDialog();

            if (timeChooseWindow.DialogResult == true)
            {
                TimeFrom = timeChooseWindow.FirstSelectedTimeControl.TimeFrom;
                TimeTo = timeChooseWindow.LastSelectedTimeControl.TimeTo;

                makeReservationButton.IsEnabled = true;
            }
        }

        private void locationChooseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Table selectedTable = (Table)locationChooseComboBox.SelectedItem;
            SelectedTable = selectedTable;

            dateChooseDateTimeUpDown.IsEnabled = true;
            dateChooseDateTimeUpDown.Focus();
        }

        private void dateChooseDateTimeUpDown_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (dateChooseDateTimeUpDown.Value != null)
            {
                DateTime selectedDate = (DateTime)dateChooseDateTimeUpDown.Value;

                SelectedDate = selectedDate;

                timeChooseButton.IsEnabled = true;
                timeChooseButton.Focus();
            }
        }
    }
}
