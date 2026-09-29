using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ShopTask.models;
using ShopTask.data;

namespace ShopTask
{
    public class CartItem : INotifyPropertyChanged
    {
        private int _quantity;
        public Product Product { get; set; }
        public int Quantity
        {
            get => _quantity;
            set { _quantity = value; OnPropertyChanged(nameof(Quantity)); OnPropertyChanged(nameof(TotalPrice)); }
        }
        public double TotalPrice => (Product != null ? Product.Price : 0) * Quantity;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public class SalesOrder
    {
        public string OrderId { get; set; }
        public DateTime Timestamp { get; set; }
        public string ItemsSummary { get; set; }
        public double TotalCost { get; set; }
    }

    public partial class ProductsWindow : Window
    {
        private string _tabItemOpenName;
        private ProductsWindow _owner;
        private List<Window> _openWindows = new List<Window>();

        public ObservableCollection<CartItem> Cart { get; set; } = new ObservableCollection<CartItem>();
        public ObservableCollection<SalesOrder> SalesHistory { get; set; } = new ObservableCollection<SalesOrder>();

        public ProductsWindow()
        {
            InitializeComponent();
            InitSystem();
        }

        public ProductsWindow(ProductsWindow owner, string tabItemName)
        {
            InitializeComponent();
            InitSystem();

            tabItemEditProducts.Visibility = Visibility.Collapsed;
            tabItemSale.Visibility = Visibility.Collapsed;
            tabItemSeller.Visibility = Visibility.Collapsed;
            tabItemTrash.Visibility = Visibility.Collapsed;
            tabItemProducts.Visibility = Visibility.Collapsed;

            if (this.FindName(tabItemName) is TabItem tabItem)
            {
                tabItem.Visibility = Visibility.Visible;
                tabItem.IsSelected = true;
            }
            _tabItemOpenName = tabItemName;
            _owner = owner;
        }

        private void InitSystem()
        {
            dgCart.ItemsSource = Cart;
            dgSalesHistory.ItemsSource = SalesHistory;

            cbCategoryFilter.Items.Clear();
            cbCategoryFilter.Items.Add("Все категории");
            cbCategoryFilter.SelectedIndex = 0;

            if (ProductsData.Products != null)
            {
                var categories = ProductsData.Products
                    .Where(p => p.Category != null)
                    .Select(p => p.Category.Name)
                    .Distinct()
                    .Where(c => !string.IsNullOrEmpty(c));

                foreach (var cat in categories) cbCategoryFilter.Items.Add(cat);
            }

            UpdateVitrinaUI();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (_tabItemOpenName != null && _owner != null)
            {
                if (_owner.FindName(_tabItemOpenName) is TabItem originalTab) originalTab.Visibility = Visibility.Visible;
                return;
            }
            _openWindows.ForEach(w => { try { w.Close(); } catch { } });
            InitData.Save();
        }

        private void WindowMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                try { DragMove(); } catch { }
            }
        }

        private void VitrinaFilter_Changed(object sender, SelectionChangedEventArgs e) => UpdateVitrinaUI();
        private void VitrinaFilter_Changed(object sender, TextChangedEventArgs e) => UpdateVitrinaUI();

        private void UpdateVitrinaUI()
        {
            if (productsPanel == null || ProductsData.Products == null) return;
            productsPanel.Children.Clear();

            var query = ProductsData.Products.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(txtSearch.Text))
                query = query.Where(p => p.Name != null && p.Name.ToLower().Contains(txtSearch.Text.ToLower()));

            // Решена проблема преобразования типов: сравниваем строку с внутренним строковым полем Name объекта Category
            if (cbCategoryFilter.SelectedIndex > 0)
            {
                string selectedCategoryStr = cbCategoryFilter.SelectedItem.ToString();
                query = query.Where(p => p.Category != null && p.Category.Name == selectedCategoryStr);
            }

            if (cbSort.SelectedIndex == 1) query = query.OrderBy(p => p.Price);
            else if (cbSort.SelectedIndex == 2) query = query.OrderByDescending(p => p.Price);
            else if (cbSort.SelectedIndex == 3) query = query.OrderByDescending(p => p.Count);

            foreach (var product in query)
            {
                productsPanel.Children.Add(CreateProductCardUI(product));
            }
        }

        private void TabControl_Selected(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl) UpdateVitrinaUI();
        }

        private Border CreateProductCardUI(Product p)
        {
            var b = new Border { Width = 210, Height = 300, Margin = new Thickness(10), CornerRadius = new CornerRadius(8), Background = (Brush)FindResource("ThemeCardBrush"), BorderThickness = new Thickness(1), BorderBrush = (Brush)FindResource("ThemeBorderBrush") };
            var sp = new StackPanel { Margin = new Thickness(10) };

            var imgBorder = new Border { Height = 120, CornerRadius = new CornerRadius(4), ClipToBounds = true, Background = Brushes.LightGray };
            imgBorder.Child = new Image { Source = p.Image, Stretch = Stretch.UniformToFill };
            sp.Children.Add(imgBorder);

            sp.Children.Add(new TextBlock { Text = p.Name, FontSize = 14, FontWeight = FontWeights.Bold, Margin = new Thickness(0, 5, 0, 2), Height = 40, TextWrapping = TextWrapping.Wrap });
            sp.Children.Add(new TextBlock { Text = $"Остаток: {p.Count} шт.", FontSize = 11, Foreground = Brushes.Gray });
            sp.Children.Add(new TextBlock { Text = $"{p.Price} руб.", FontSize = 16, FontWeight = FontWeights.Black, Foreground = (Brush)FindResource("ThemeAccentBrush"), Margin = new Thickness(0, 5, 0, 5) });

            var btn = new Button { Content = "🛒 В корзину", FontWeight = FontWeights.Bold };
            btn.Click += (s, ev) => AddToCart(p);
            sp.Children.Add(btn);

            b.Child = sp;
            return b;
        }

        private void AddToCart(Product p)
        {
            if (p.Count <= 0)
            {
                MessageBox.Show("Товара нет в наличии!", "Склад");
                return;
            }

            var exist = Cart.FirstOrDefault(c => c.Product.Id == p.Id);
            if (exist != null)
            {
                if (exist.Quantity < p.Count) exist.Quantity++;
                else MessageBox.Show("Нельзя заказать больше остатка!", "Внимание");
            }
            else
            {
                Cart.Add(new CartItem { Product = p, Quantity = 1 });
            }
            UpdateCartSummary();
        }

        private void BtnCartMinus_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).DataContext is CartItem item)
            {
                if (item.Quantity > 1) item.Quantity--;
                else Cart.Remove(item);
                UpdateCartSummary();
            }
        }

        private void BtnCartPlus_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).DataContext is CartItem item)
            {
                if (item.Quantity < item.Product.Count) item.Quantity++;
                else MessageBox.Show("Достигнут лимит склада!", "Внимание");
                UpdateCartSummary();
            }
        }

        private void BtnCartRemove_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button).DataContext is CartItem item) { Cart.Remove(item); UpdateCartSummary(); }
        }

        private void UpdateCartSummary()
        {
            txtCartCount.Text = Cart.Sum(c => c.Quantity).ToString();
            txtCartTotal.Text = $"{Cart.Sum(c => c.TotalPrice)} руб.";
        }

        private void BtnCheckout_Click(object sender, RoutedEventArgs e)
        {
            if (!Cart.Any()) return;

            string summary = string.Join(", ", Cart.Select(c => $"{c.Product.Name} (x{c.Quantity})"));
            double cost = Cart.Sum(c => c.TotalPrice);

            foreach (var item in Cart) item.Product.Count -= item.Quantity;

            SalesHistory.Add(new SalesOrder
            {
                OrderId = $"ЧЕК-{Guid.NewGuid().ToString().Substring(0, 5).ToUpper()}",
                Timestamp = DateTime.Now,
                ItemsSummary = summary,
                TotalCost = cost
            });

            Cart.Clear();
            UpdateCartSummary();
            UpdateStats();
            UpdateVitrinaUI();
            dataGridProductsEdit.Items.Refresh();
            MessageBox.Show("Покупка успешно оформлена!", "Касса");
        }

        private void UpdateStats()
        {
            double totalRev = SalesHistory.Sum(s => s.TotalCost);
            int totalItems = SalesHistory.Count;
            statRevenue.Text = $"{totalRev} руб.";
            statItemsCount.Text = $"{totalItems} опер.";
            statAverageBill.Text = totalItems > 0 ? $"{(int)(totalRev / totalItems)} руб." : "0 руб.";
        }

        private void ButtonAddProduct_Click(object sender, RoutedEventArgs e)
        {
            var newProduct = new Product() { Name = "Новый товар", Price = 0, Count = 0, Category = ProductsData.Categories.FirstOrDefault() };
            ProductsData.Products.Add(newProduct);
            UpdateVitrinaUI();
        }

        private void Button_Click_ImageChange(object sender, RoutedEventArgs e)
        {
            if (dataGridProductsEdit.SelectedItem is Product product)
            {
                var openFileDialog = new Microsoft.Win32.OpenFileDialog { Filter = "Изображения|*.jpg;*.jpeg;*.png" };
                if (openFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        string dirPath = "images";
                        Directory.CreateDirectory(dirPath);
                        var newPath = Path.Combine(dirPath, product.Id + Path.GetFileName(openFileDialog.FileName));
                        if (File.Exists(newPath)) File.Delete(newPath);
                        File.Copy(openFileDialog.FileName, newPath);
                        product.ImagePath = newPath;
                        UpdateVitrinaUI();
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                }
            }
        }

        private void Button_Click_RemoveProduct(object sender, RoutedEventArgs e)
        {
            if (dataGridProductsEdit.SelectedItem is Product p) { ProductsData.Products.Remove(p); UpdateVitrinaUI(); }
        }

        private void SliderSizeLabel_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sliderSizeLabel == null) return;
            Style style = new Style(typeof(TextBlock), Application.Current.Resources[typeof(TextBlock)] as Style);
            style.Setters.Add(new Setter(TextBlock.FontSizeProperty, sliderSizeLabel.Value));
            Application.Current.Resources[typeof(TextBlock)] = style;
        }

        private void ChangeThemePalette(string bg, string card, string text, string accent, string border)
        {
            Application.Current.Resources["ThemeBgBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(bg));
            Application.Current.Resources["ThemeCardBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(card));
            Application.Current.Resources["ThemeTextBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(text));
            Application.Current.Resources["ThemeAccentBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(accent));
            Application.Current.Resources["ThemeBorderBrush"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(border));
        }

        private void Theme_Click(object sender, RoutedEventArgs e)
        {
            string themeTag = (sender as Button)?.Tag?.ToString();
            switch (themeTag)
            {
                case "Classic": ChangeThemePalette("#F0F4F8", "#FFFFFF", "#1C1C1E", "#007AFF", "#E5E5EA"); break;
                case "DeepDark": ChangeThemePalette("#0F0F12", "#1E1E24", "#F5F5F7", "#3A86FF", "#2A2A32"); break;
                case "Summer": ChangeThemePalette("#E8F5E9", "#FFFFFF", "#1B5E20", "#4CAF50", "#C8E6C9"); break;
                case "Cyberpunk": ChangeThemePalette("#121214", "#1F1F23", "#00FFFF", "#FF0055", "#F8E71C"); break;
                case "Matrix": ChangeThemePalette("#000000", "#0D0D0D", "#00FF00", "#008000", "#333333"); break;
                case "Dracula": ChangeThemePalette("#282A36", "#44475A", "#F8F8F2", "#BD93F9", "#6272A4"); break;
            }
            UpdateVitrinaUI();
        }
    }
}