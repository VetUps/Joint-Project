using Restaurant.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для RedacOrDeleteFeedbackWindow.xaml
    /// </summary>
    public partial class RedacOrDeleteFeedbackWindow : Window
    {
        public RedacOrDeleteFeedbackWindow(Feedback feedback)
        {
            InitializeComponent();

            CurrentFeedback = feedback;

            feedbackTextBox.Text = CurrentFeedback.FeedbackText;
        }

        private Feedback CurrentFeedback { get; set; }

        private List<string> BadWords = new List<string>()
        {
            "блин",
            "черт",
            "сволочь",
            "дурак",
            "мудак",
            "гад",
            "козел",
            "идиот",
            "тупица",
            "недоумок",
            "жопа",
            "дебил",
            "крипота",
            "гнида",
            "Ужасно"
        };

        private string ModerateText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            string replaceWord = new string('*', text.Length);
            var words = Regex.Split(text, @"\W+");

            if (words.Any(word => BadWords.Contains(word)))
                text = text.Replace(text, replaceWord, StringComparison.OrdinalIgnoreCase);

            return text;
        }

        private void SaveModerateText(string text)
        {
            if (text == CurrentFeedback.FeedbackText)
            {
                MessageBox.Show("Отзыв в модерации не нуждается", 
                                "Модерация", 
                                MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Feedback ModerateFeedback = new Feedback();

            ModerateFeedback.FeedbackText = text;
            ModerateFeedback.FeedbackDate = CurrentFeedback.FeedbackDate;
            ModerateFeedback.Rating = CurrentFeedback.Rating;

            using (var db = new RestaurantDbContext())
            {
                if (CurrentFeedback != null)
                {
                    ModerateFeedback.FeedbackId = CurrentFeedback.FeedbackId;
                    db.Update(CurrentFeedback).CurrentValues.SetValues(ModerateFeedback);
                }
                db.SaveChanges();
            }

            feedbackTextBox.Text = ModerateFeedback.FeedbackText;
            MessageBox.Show("Модерация прошла успешно", 
                            "Модерация", 
                            MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void moderationButton_Click(object sender, RoutedEventArgs e)
        {
            string ModText = ModerateText(feedbackTextBox.Text);

            if (!string.IsNullOrEmpty(ModText))
            {
                SaveModerateText(ModText);
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
