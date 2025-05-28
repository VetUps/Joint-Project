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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Restaurant.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для FeedbackUserControl.xaml
    /// </summary>
    public partial class FeedbackUserControl : UserControl
    {
        public FeedbackUserControl(Feedback userFeedback)
        {
            InitializeComponent();

            FullFeedbackInfo = userFeedback;
            UserFeedback = new FeedbackViewModel
            {
                Text = userFeedback.FeedbackText,
                Rating = (int)userFeedback.Rating,
                Date = string.Format("{0:dd.MM.yyyy}", userFeedback.FeedbackDate)
            };

            DataContext = UserFeedback;
        }

        public Feedback FullFeedbackInfo { get; private set; }
        public FeedbackViewModel UserFeedback { get; set; }

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
    }
}
