using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Media.Imaging;

namespace TaskInWPF
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<Game> games;
        private readonly string jsonFile = "games.json";
        private Game currentEditGame = null;

        public MainWindow()
        {
            InitializeComponent();
            LoadData();
            ClearForm();
            btnDelete.IsEnabled = false;
        }

        private void LoadData()
        {
            if (File.Exists(jsonFile))
            {
                string json = File.ReadAllText(jsonFile);
                var list = JsonSerializer.Deserialize<ObservableCollection<Game>>(json);
                games = list ?? new ObservableCollection<Game>();
            }
            else
            {
                games = new ObservableCollection<Game>();
            }
            dgGames.ItemsSource = games;
        }

        private void SaveToJson()
        {
            string json = JsonSerializer.Serialize(games, new JsonSerializerOptions { WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping

            });
            File.WriteAllText(jsonFile, json);
            MessageBox.Show("Данные сохранены в JSON", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ClearForm()
        {
            txtTitle.Text = "";
            txtDescription.Text = "";
            txtRating.Text = "";
            chkIsFree.IsChecked = false;
            txtPrice.Text = "";
            txtAge.Text = "";
            txtGenre.Text = "";
            txtCoverPath.Text = "";
            imgPreview.Source = null;
            currentEditGame = null;
            btnAddUpdate.Content = "Добавить";
            btnDelete.IsEnabled = false;
        }

        private void LoadGameToForm(Game game)
        {
            txtTitle.Text = game.Title;
            txtDescription.Text = game.Description;
            txtRating.Text = game.Rating.ToString();
            chkIsFree.IsChecked = game.IsFree;
            txtPrice.Text = game.IsFree ? "" : game.Price.ToString();
            txtAge.Text = game.AgeRestriction.ToString();
            txtGenre.Text = game.Genre;
            txtCoverPath.Text = game.CoverImagePath;
            if (!string.IsNullOrEmpty(game.CoverImagePath) && File.Exists(game.CoverImagePath))
                imgPreview.Source = new BitmapImage(new Uri(game.CoverImagePath));
            else
                imgPreview.Source = null;
            currentEditGame = game;
            btnAddUpdate.Content = "Обновить";
            btnDelete.IsEnabled = true;
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            { MessageBox.Show("Введите название"); return false; }
            if (string.IsNullOrWhiteSpace(txtGenre.Text))
            { MessageBox.Show("Введите жанр"); return false; }
            if (!double.TryParse(txtRating.Text, out double rating) || rating < 0 || rating > 10)
            { MessageBox.Show("Рейтинг от 0 до 10"); return false; }
            if (!int.TryParse(txtAge.Text, out int age) || age < 0)
            { MessageBox.Show("Возрастное ограничение – положительное число"); return false; }
            if (chkIsFree.IsChecked != true)
            {
                if (!decimal.TryParse(txtPrice.Text, out decimal price) || price <= 0)
                { MessageBox.Show("Введите цену > 0"); return false; }
            }
            return true;
        }

        private void btnAddUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateFields()) return;

            if (currentEditGame == null)
            {
                // Добавление
                int newId = games.Count > 0 ? games.Max(g => g.Id) + 1 : 1;
                var game = new Game
                {
                    Id = newId,
                    Title = txtTitle.Text.Trim(),
                    Description = txtDescription.Text.Trim(),
                    Rating = double.Parse(txtRating.Text),
                    IsFree = chkIsFree.IsChecked == true,
                    Price = (chkIsFree.IsChecked == true) ? 0 : decimal.Parse(txtPrice.Text),
                    AgeRestriction = int.Parse(txtAge.Text),
                    Genre = txtGenre.Text.Trim(),
                    CoverImagePath = txtCoverPath.Text.Trim()
                };
                games.Add(game);
                SaveToJson();
                ClearForm();
            }
            else
            {
                // Редактирование
                var confirm = MessageBox.Show($"ID: {currentEditGame.Id}\nИмя: {currentEditGame.Title}\nЦена: {currentEditGame.DisplayPrice}\nОбновить игру?",
                    "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (confirm == MessageBoxResult.Yes)
                {
                    currentEditGame.Title = txtTitle.Text.Trim();
                    currentEditGame.Description = txtDescription.Text.Trim();
                    currentEditGame.Rating = double.Parse(txtRating.Text);
                    currentEditGame.IsFree = chkIsFree.IsChecked == true;
                    currentEditGame.Price = (chkIsFree.IsChecked == true) ? 0 : decimal.Parse(txtPrice.Text);
                    currentEditGame.AgeRestriction = int.Parse(txtAge.Text);
                    currentEditGame.Genre = txtGenre.Text.Trim();
                    currentEditGame.CoverImagePath = txtCoverPath.Text.Trim();
                    SaveToJson();
                    ClearForm();
                    dgGames.Items.Refresh();
                }
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (currentEditGame == null) return;
            var confirm = MessageBox.Show($"ID: {currentEditGame.Id}\nИмя: {currentEditGame.Title}\nЦена: {currentEditGame.DisplayPrice}\nУдалить игру?",
                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm == MessageBoxResult.Yes)
            {
                games.Remove(currentEditGame);
                SaveToJson();
                ClearForm();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void dgGames_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (dgGames.SelectedItem is Game selected)
                LoadGameToForm(selected);
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog { Filter = "Image files|*.jpg;*.png;*.bmp;*.jpeg" };
            if (dlg.ShowDialog() == true)
            {
                txtCoverPath.Text = dlg.FileName;
                imgPreview.Source = new BitmapImage(new Uri(dlg.FileName));
            }
        }

        private void chkIsFree_Checked(object sender, RoutedEventArgs e)
        {
            txtPrice.IsEnabled = chkIsFree.IsChecked != true;
            if (chkIsFree.IsChecked == true)
                txtPrice.Text = "";
        }
    }

    public class Game : System.ComponentModel.INotifyPropertyChanged
    {
        private int id;
        private string title;
        private string description;
        private double rating;
        private bool isFree;
        private decimal price;
        private int ageRestriction;
        private string genre;
        private string coverImagePath;

        public int Id { get => id; set { id = value; OnPropertyChanged(nameof(Id)); } }
        public string Title { get => title; set { title = value; OnPropertyChanged(nameof(Title)); } }
        public string Description { get => description; set { description = value; OnPropertyChanged(nameof(Description)); } }
        public double Rating { get => rating; set { rating = value; OnPropertyChanged(nameof(Rating)); } }
        public bool IsFree { get => isFree; set { isFree = value; OnPropertyChanged(nameof(IsFree)); OnPropertyChanged(nameof(DisplayPrice)); } }
        public decimal Price { get => isFree ? 0 : price; set { price = value; OnPropertyChanged(nameof(Price)); OnPropertyChanged(nameof(DisplayPrice)); } }
        public int AgeRestriction { get => ageRestriction; set { ageRestriction = value; OnPropertyChanged(nameof(AgeRestriction)); } }
        public string Genre { get => genre; set { genre = value; OnPropertyChanged(nameof(Genre)); } }
        public string CoverImagePath { get => coverImagePath; set { coverImagePath = value; OnPropertyChanged(nameof(CoverImagePath)); } }
        public string DisplayPrice => IsFree ? "Бесплатно" : $"{Price:C}";

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(name));
    }
}