using LiveCharts.Wpf;
using LiveCharts;
using Restaurant.Classes;
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

namespace Restaurant.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для AdminReportPage.xaml
    /// </summary>
    public partial class AdminReportPage : Page
    {
        private readonly RestaurantAnalyticsService _analyticsService;
        private List<Category> _categories;
        private List<PopularDish> _allPopularDishes;
        private int? _selectedCategoryId;

        public SeriesCollection RatingSeries { get; set; }
        public string[] RatingLabels { get; set; }
        public Func<double, string> Formatter { get; set; }
        public IEnumerable<PopularDish> PopularDishes { get; set; }
        public IEnumerable<Category> Categories => _categories;
        public int? SelectedCategoryId
        {
            get => _selectedCategoryId;
            set
            {
                _selectedCategoryId = value;
                FilterPopularDishes();
            }
        }
        public ICommand ShowAllDishesCommand { get; }

        public AdminReportPage()
        {
            InitializeComponent();
            _analyticsService = new RestaurantAnalyticsService(new RestaurantDbContext());
            ShowAllDishesCommand = new RelayCommand(_ => ShowAllDishes());
            DataContext = this;
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                _allPopularDishes = _analyticsService.GetPopularDishesByCategory().ToList();
                _categories = _analyticsService.GetCategories().ToList();
                var restaurantRating = _analyticsService.GetRestaurantRating();

                PopularDishes = _allPopularDishes;
                UpdateRatingChart(restaurantRating);
                DisplayRatingStats(restaurantRating);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FilterPopularDishes()
        {
            if (_selectedCategoryId == null)
            {
                PopularDishes = _allPopularDishes;
            }
            else
            {
                PopularDishes = _allPopularDishes
                    .Where(d => d.CategoryId == _selectedCategoryId.Value)
                    .ToList();
            }

            // Notify UI about changes
            PopularDishesGrid.ItemsSource = PopularDishes;
            PopularDishesGrid.Items.Refresh();
        }

        private void ShowAllDishes()
        {
            SelectedCategoryId = null;
            CategoryComboBox.SelectedItem = null;
        }

        private void UpdateRatingChart(RestaurantRatingSummary rating)
        {
            RatingSeries = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Количество отзывов",
                    Values = new ChartValues<int>
                    {
                        rating.OneStarCount,
                        rating.TwoStarCount,
                        rating.ThreeStarCount,
                        rating.FourStarCount,
                        rating.FiveStarCount
                    }
                }
            };

            RatingLabels = new[] { "1 звезда", "2 звезды", "3 звезды", "4 звезды", "5 звезд" };
            Formatter = value => value.ToString("N0");

            RatingChart.Series = RatingSeries;
            RatingChart.AxisX[0].Labels = RatingLabels;
            RatingChart.AxisY[0].LabelFormatter = Formatter;
        }

        private void DisplayRatingStats(RestaurantRatingSummary rating)
        {
            AverageRatingText.Text = $"Средний рейтинг: {rating.AverageRating?.ToString("F1") ?? "0"} из 5";
            TotalFeedbacksText.Text = $"Всего отзывов: {rating.TotalFeedbacks}";

            RatingStatsPanel.Children.Clear();

            AddRatingStatItem("5 звезд", rating.FiveStarCount, rating.FiveStarPercentage);
            AddRatingStatItem("4 звезды", rating.FourStarCount, rating.FourStarPercentage);
            AddRatingStatItem("3 звезды", rating.ThreeStarCount, rating.ThreeStarPercentage);
            AddRatingStatItem("2 звезды", rating.TwoStarCount, rating.TwoStarPercentage);
            AddRatingStatItem("1 звезда", rating.OneStarCount, rating.OneStarPercentage);
        }

        private void AddRatingStatItem(string label, int count, double percentage)
        {
            var stackPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 5, 0, 0) };

            stackPanel.Children.Add(new TextBlock
            {
                Text = $"{label}: ",
                FontWeight = FontWeights.Bold,
                Width = 70
            });

            stackPanel.Children.Add(new TextBlock
            {
                Text = $"{count} ({percentage:F1}%)",
                Width = 100
            });

            var progressBar = new ProgressBar
            {
                Value = percentage,
                Maximum = 100,
                Width = 200,
                Height = 15,
                Margin = new Thickness(10, 0, 0, 0)
            };

            stackPanel.Children.Add(progressBar);

            RatingStatsPanel.Children.Add(stackPanel);
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => _canExecute == null || _canExecute(parameter);

        public void Execute(object parameter) => _execute(parameter);

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }
}
