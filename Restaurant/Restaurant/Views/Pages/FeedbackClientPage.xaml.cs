using Restaurant.Models;
using Restaurant.Views.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Restaurant.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для FeedbackCientPage.xaml
    /// </summary>
    public partial class FeedbackCientPage : Page
    {
        public FeedbackCientPage()
        {
            InitializeComponent();
            DataContext = this;
            LoadFeedbacks();
        }

        public ObservableCollection<FeedbackViewModel> Feedbacks { get; set; } = new ObservableCollection<FeedbackViewModel>();
        public class FeedbackViewModel
        {
            public string Text { get; set; }
            public int Rating { get; set; }
            public string Date { get; set; }

            public bool Star1 => Rating >= 1;
            public bool Star2 => Rating >= 2;
            public bool Star3 => Rating >= 3;
            public bool Star4 => Rating >= 4;
            public bool Star5 => Rating >= 5;
        }

        public void LoadFeedbacks()
        {
            using (var db = new RestaurantDbContext())
            {
                var feedbacks = db.Feedbacks
                    .OrderByDescending(f => f.FeedbackDate)
                    .Select(f => new FeedbackViewModel
                    {
                        Text = f.FeedbackText,
                        Rating = (int)f.Rating,
                        Date = string.Format("{0:dd.MM.yyyy}", f.FeedbackDate)
                    })
                    .ToList();

                FeedbackContainer.ItemsSource = feedbacks;
            }
        }

        private void LeaveFeedback_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var leaveFeedbackForm = new LeaveFeedbackWindow();
            leaveFeedbackForm.FeedbackSaved += () => LoadFeedbacks();
            leaveFeedbackForm.ShowDialog();
        }
    }
}
