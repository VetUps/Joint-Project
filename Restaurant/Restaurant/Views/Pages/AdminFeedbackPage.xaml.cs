using Restaurant.Models;
using Restaurant.Views.UserControls;
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
    /// Логика взаимодействия для AdminFeedbackPage.xaml
    /// </summary>
    public partial class AdminFeedbackPage : Page
    {
        public AdminFeedbackPage()
        {
            InitializeComponent();

            DataContext = this;
            LoadFeedBack();
        }

        private ObservableCollection<FeedbackUserControl> _feedbackCards = new();
        public ObservableCollection<FeedbackUserControl> FeedbackCardSource
        {
            get => _feedbackCards;
            set
            {
                _feedbackCards = value;
            }
        }

        private void LoadFeedBack()
        {
            try
            {
                using (var context = new RestaurantDbContext())
                {
                    var feedbacks = context.Feedbacks.ToList();
                    FeedbackCardSource.Clear();

                    foreach (var feedback in feedbacks)
                    {
                        FeedbackCardSource.Add(new FeedbackUserControl(feedback));
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при попытке загрузке данных об отзывах.\nПодробнее:\n{ex.Message}", 
                                 "Ошибка!", 
                                 MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void feedbackListBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (feedbackListBox.SelectedItem is FeedbackUserControl feedbackControl)
            {
                var feedback = feedbackControl.FullFeedbackInfo;
                RedacOrDeleteFeedbackWindow redac = new RedacOrDeleteFeedbackWindow(feedback);

                if (redac.ShowDialog() == true)
                {
                    FeedbackCardSource.Clear();
                    LoadFeedBack();
                }
            }
        }
    }
}
