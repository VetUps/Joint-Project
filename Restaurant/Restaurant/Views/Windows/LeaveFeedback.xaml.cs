using Restaurant.Models;
using Restaurant.Views.Pages;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Логика взаимодействия для LeaveFeedback.xaml
    /// </summary>
    public partial class LeaveFeedback : Window
    {
        private Feedback feedback;
        public event Action FeedbackSaved;
        public LeaveFeedback()
        {
            InitializeComponent();
        }
        private void Star_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock clickedStar = (TextBlock)sender;
            int starNumber = int.Parse(clickedStar.Tag.ToString());

            SetStarsColor(starNumber);
        }

        private void SetStarsColor(int upToStar)
        {
            Star1.Foreground = Brushes.LightGray;
            Star2.Foreground = Brushes.LightGray;
            Star3.Foreground = Brushes.LightGray;
            Star4.Foreground = Brushes.LightGray;
            Star5.Foreground = Brushes.LightGray;

            if (upToStar >= 1) Star1.Foreground = Brushes.Gold;
            if (upToStar >= 2) Star2.Foreground = Brushes.Gold;
            if (upToStar >= 3) Star3.Foreground = Brushes.Gold;
            if (upToStar >= 4) Star4.Foreground = Brushes.Gold;
            if (upToStar >= 5) Star5.Foreground = Brushes.Gold;
        }

        private void CommentText_GotFocus(object sender, RoutedEventArgs e)
        {
            if (CommentText.Text == "Напишите комментарий...")
            {
                CommentText.Text = "";
                CommentText.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#DF8C22");
            }
        }

        private void CommentText_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CommentText.Text))
            {
                CommentText.Text = "Напишите комментарий...";
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private int GetSelectedRating()
        {
            if (Star5.Foreground == Brushes.Gold) return 5;
            if (Star4.Foreground == Brushes.Gold) return 4;
            if (Star3.Foreground == Brushes.Gold) return 3;
            if (Star2.Foreground == Brushes.Gold) return 2;
            if (Star1.Foreground == Brushes.Gold) return 1;
            return 0;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var db = new RestaurantDbContext())
                {
                    feedback = new Feedback();
                    feedback.FeedbackText = CommentText.Text;
                    feedback.FeedbackDate = DateOnly.FromDateTime(VisitDatePicker.SelectedDate.Value);
                    feedback.Rating = GetSelectedRating();
                    db.Feedbacks.Add(feedback);
                    db.SaveChanges();
                    MessageBox.Show("Спасибо за отзыв!", "Уведомление", MessageBoxButton.OK);
                    
                }
                FeedbackSaved?.Invoke();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Произошла ошибка при добавлении отзыва", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ChooseDate_Click(object sender, RoutedEventArgs e)
        {
            VisitDatePicker.DisplayDateStart = new DateTime(2025, 1, 1);
            VisitDatePicker.DisplayDateEnd = DateTime.Now;
            if (!VisitDatePicker.SelectedDate.HasValue)
            {
                VisitDatePicker.SelectedDate = DateTime.Now;
            }

            VisitDatePicker.Visibility = Visibility.Visible;
            VisitDatePicker.IsDropDownOpen = true;
        }

        private void VisitDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VisitDatePicker.SelectedDate.HasValue)
            {
                ChooseDate.Content = VisitDatePicker.SelectedDate.Value.ToString("dd.MM.yyyy");
                VisitDatePicker.Visibility = Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите дату посещения", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
