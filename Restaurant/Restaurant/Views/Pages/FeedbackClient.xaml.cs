using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Restaurant.Models;
using Restaurant.Views.Windows;

namespace Restaurant.Views.Pages
{
    public partial class FeedbackClient : Page
    {
        public ObservableCollection<FeedbackViewModel> Feedbacks { get; set; } = new ObservableCollection<FeedbackViewModel>();

        public FeedbackClient()
        {
            InitializeComponent();
            DataContext = this;
            LoadFeedbacks();
        }

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
            var leaveFeedbackForm = new LeaveFeedback();
            leaveFeedbackForm.FeedbackSaved += () => LoadFeedbacks();
            leaveFeedbackForm.Show();
        }
    }
}