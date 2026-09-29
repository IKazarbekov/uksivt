using ShopTask.data;
using ShopTask.models;
using ShopTask2;
using ShopTask2.data;
using ShopTask2.utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace ShopTask
{
    /// <summary>
    /// Логика взаимодействия для ProductsWindow.xaml
    /// </summary>
    public partial class ProductsWindow : Window
    {
        public User user { get; set; }
        public ProductsWindow()
        {
            InitializeComponent();
            RadioButtonThemeClassic_Click(this, new RoutedEventArgs());
            dataGridUser.ItemsSource = UsersData.Users;
            dataGridSales.ItemsSource = SellerData.Sales;
            dataGridProductsEdit.ItemsSource = ProductsData.Products;
            UpdateButtonSeller();
        }

        // Для входа продавца или админа явно
        public ProductsWindow(User user)
        {
            InitializeComponent();
            RadioButtonThemeClassic_Click(this, new RoutedEventArgs());
            dataGridSales.ItemsSource = SellerData.Sales;
            this.user = user;

            if (user.role == Role.Seller)
            {
                tabItemEditProducts.Visibility = Visibility.Collapsed;
                tabItemProducts.Visibility = Visibility.Visible;
                tabItemSeller.Visibility = Visibility.Collapsed;
                
            }
        }

        // Для открытия лишь одной вкладки
        private string tabItemOpenName;
        ProductsWindow owner;
        List<Window> openWindows = new List<Window>();
        public ProductsWindow(ProductsWindow owner, string tabItemName)
        {
            InitializeComponent();
            RadioButtonThemeClassic_Click(this, new RoutedEventArgs());
            dataGridSales.ItemsSource = SellerData.Sales;
            dataGridUser.ItemsSource = UsersData.Users;
            dataGridProductsEdit.ItemsSource = ProductsData.Products;
            tabItemEditProducts.Visibility = Visibility.Collapsed;
            tabItemSale.Visibility = Visibility.Collapsed;
            tabItemSeller.Visibility = Visibility.Collapsed;
            tabItemProducts.Visibility = Visibility.Collapsed;

            var tabItem = this.FindName(tabItemName) as TabItem;
                tabItem.Visibility = Visibility.Visible;
                tabItem.IsSelected = true;
            tabItemOpenName = tabItemName;
            this.owner = owner;
            labelSettings.Visibility = Visibility.Collapsed;
            buttonLogout.Visibility = Visibility.Collapsed;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (tabItemOpenName != null)
            {
                ((TabItem)owner.FindName(tabItemOpenName)).Visibility = Visibility.Visible;
                return;
            }
            openWindows.ForEach(w => w.Close());
            InitData.Save();
        }
        private void Window_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch
            {
                // возникает при нажатии правой кнопкой мыши
            }
        }
        private void TabControl_Selected(object sender, RoutedEventArgs e)
        {
            var tabControl = (TabControl)sender;
            var tabItem = (TabItem)tabControl.SelectedItem;
            if (tabItem == null)
                return;

            switch (tabItem.Header.ToString())
            {
                case "Товары":
                    productsPanel.Children.Clear();
                    foreach (var product in ProductsData.Products)
                        productsPanel.Children.Add(new ProductControl(product));
                    break;
                case "-":
                    Close();
                    break;
                case "[ ]":
                    MessageBox.Show(this.WindowState.ToString());
                    if (this.WindowState == WindowState.Maximized)
                    {
                        MessageBox.Show("1");
                        this.WindowState = WindowState.Normal;
                        Height = 800;
                        Width = 1300;
                    }
                    else
                        this.WindowState = WindowState.Maximized;
                    break;
            }
        }
        private void ButtonAddProduct_Click(object sender, RoutedEventArgs e)
        {
            var newProduct = new Product() { Name="Новый продукт"};
            int[] ids = ProductsData.Products.Select(p => p.Id).ToArray();
            int newIndex = newProduct.Count != 0 ? ids.Max() + 1 : 0;
            newProduct.Id = newIndex;
            ProductsData.Products.Add(newProduct);
        }
        private void Button_Click_ImageChange(object sender, RoutedEventArgs e)
        {
            var product = dataGridProductsEdit.SelectedItem as Product;
            if (!FileDialog.OpenImage(out string path))
                return;
            Directory.CreateDirectory(Config.NAME_DIR_IMAGES);
            var newPath = $"{Config.NAME_DIR_IMAGES}/" + product.Id + Path.GetFileName(path);
            File.Delete(newPath);
            File.Copy(path, newPath);
            product.ImagePath = newPath;
            new ProductsWindow() {Height=Height, Width=Width, WindowState = WindowState, Top = Top, Left = Left}.Show();
            Close();
        }
        private void Button_Click_RemoveProduct(object sender, RoutedEventArgs e)
        {
            var product = dataGridProductsEdit.SelectedItem as Product;
            ProductsData.Products.Remove(product);
        }
        private void Button_Click_CloseWindow(object sender, RoutedEventArgs e) => Close();
        private void Button_Click_ChangeSizeWindow(object sender, RoutedEventArgs e) => WindowState = (WindowState == WindowState.Maximized) ? WindowState.Normal : WindowState.Maximized;
        private void Button_Click_HiddenWindow(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
        private void RadioButtonThemeClassic_Click(object sender, RoutedEventArgs e) => SetTheme(Brushes.AliceBlue, Brushes.White, Brushes.Black);
        private void RadioButtonThemeDark_Click(object sender, RoutedEventArgs e) => SetTheme(Brushes.Black, new SolidColorBrush(Color.FromRgb(20, 20, 20)), Brushes.LightGray);
        private void RadioButtonThemeSummer_Click(object sender, RoutedEventArgs e) => SetTheme(Brushes.DarkGreen, Brushes.Green, Brushes.Blue);
        private void RadioButtonThemeAlpha_Click(object sender, RoutedEventArgs e) => SetTheme(new SolidColorBrush(Color.FromArgb(40, 0, 0, 50)), new SolidColorBrush(Color.FromArgb(70, 0, 0, 50)), Brushes.Blue);
        private void TabItem_OutTab(object sender, MouseButtonEventArgs e)
        {
            // Вынесение вкладки в другое окно
            var tabItem = sender as TabItem;
            tabItem.Visibility = Visibility.Collapsed;
            tabItem.IsSelected = false;
            new ProductsWindow(this, tabItem.Name).Show();
        }
        private void SliderSizeLabel_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = sender as Slider;
            Style oldStyle = Application.Current.Resources[typeof(TextBlock)] as Style;
            Style style = new Style(typeof(TextBlock), oldStyle);
            style.Setters.Add(new Setter(TextBlock.FontSizeProperty, slider.Value));
            Application.Current.Resources[typeof(TextBlock)] = style;
        }

        // System methods
        private void ReloadWindow()
        {
            new ProductsWindow() { Height = Height, Width = Width, WindowState = WindowState, Top = Top, Left = Left}.Show();
            Close();
        }
        private void SetTheme(Brush fonColor, Brush frontColor, Brush textColor)
        {
            // Stack panel
            Style styleStackPanel = (Style)Application.Current.Resources[typeof(StackPanel)];
            var newStyleStackPanel = new Style(typeof(StackPanel), styleStackPanel);
            newStyleStackPanel.Setters.Add(new Setter(StackPanel.BackgroundProperty, fonColor));
            Application.Current.Resources[typeof(StackPanel)] = newStyleStackPanel;

            // Border
            Style styleBorder = (Style)Application.Current.Resources[typeof(Border)];
            var newStyleBorder = new Style(typeof(Border), styleBorder);
            newStyleBorder.Setters.Add(new Setter(Border.BackgroundProperty, frontColor));
            newStyleBorder.Setters.Add(new Setter(Border.BorderBrushProperty, frontColor));
            Application.Current.Resources[typeof(Border)] = newStyleBorder;

            // TabControl
            Style styleTabControl = (Style)Application.Current.Resources[typeof(TabControl)];
            var newStyleTabControl = new Style(typeof(TabControl), styleTabControl);
            newStyleTabControl.Setters.Add(new Setter(TabControl.BackgroundProperty, frontColor));
            newStyleTabControl.Setters.Add(new Setter(TabControl.BorderBrushProperty, frontColor));
            Application.Current.Resources[typeof(TabControl)] = newStyleTabControl;

            // TabItem
            Style styleTabItem = (Style)Application.Current.Resources[typeof(TabItem)];
            var newStyleTabItem = new Style(typeof(TabItem), styleTabItem);
            newStyleTabItem.Setters.Add(new Setter(TabItem.BackgroundProperty, frontColor));
            newStyleTabItem.Setters.Add(new Setter(TabItem.BorderBrushProperty, frontColor));
            newStyleTabItem.Setters.Add(new Setter(TabItem.ForegroundProperty, textColor));
            Application.Current.Resources[typeof(TabItem)] = newStyleTabItem;

            // DataGrid
            Style styleDataGrid = (Style)Application.Current.Resources[typeof(DataGrid)];
            var newStyleDataGrid = new Style(typeof(DataGrid), styleDataGrid);
            newStyleDataGrid.Setters.Add(new Setter(DataGrid.BackgroundProperty, frontColor));
            newStyleDataGrid.Setters.Add(new Setter(DataGrid.BorderBrushProperty, frontColor));
            newStyleDataGrid.Setters.Add(new Setter(DataGrid.ForegroundProperty, textColor));
            newStyleDataGrid.Setters.Add(new Setter(DataGrid.RowBackgroundProperty, fonColor));
            Application.Current.Resources[typeof(DataGrid)] = newStyleDataGrid;

            // Button
            Style styleButton = (Style)Application.Current.Resources[typeof(Button)];
            var newStyleButton = new Style(typeof(Button), styleButton);
            newStyleButton.Setters.Add(new Setter(Button.BackgroundProperty, frontColor));
            newStyleButton.Setters.Add(new Setter(Button.BorderBrushProperty, frontColor));
            newStyleButton.Setters.Add(new Setter(Button.ForegroundProperty, textColor));
            Application.Current.Resources[typeof(Button)] = newStyleButton;

            // ComboBox
            Style styleComboBox = (Style)Application.Current.Resources[typeof(ComboBox)];
            var newStyleComboBox = new Style(typeof(ComboBox), styleComboBox);
            newStyleComboBox.Setters.Add(new Setter(ComboBox.BackgroundProperty, frontColor));
            newStyleComboBox.Setters.Add(new Setter(ComboBox.BorderBrushProperty, frontColor));
            newStyleComboBox.Setters.Add(new Setter(ComboBox.ForegroundProperty, textColor));
            Application.Current.Resources[typeof(ComboBox)] = newStyleComboBox;

            // Window
            Style styleWindow = (Style)Application.Current.Resources["window"];
            var newStyleWindow = new Style(typeof(Window), styleWindow);
            newStyleWindow.Setters.Add(new Setter(Window.BackgroundProperty, fonColor));
            Application.Current.Resources["window"] = newStyleWindow;

            // Label
            Style styleLabel = (Style)Application.Current.Resources[typeof(Label)];
            var newStyleLabel = new Style(typeof(Label), styleLabel);
            newStyleLabel.Setters.Add(new Setter(Label.ForegroundProperty, textColor));
            Application.Current.Resources[typeof(Label)] = newStyleLabel;

            // TextBlock
            Style styleTextBlock = (Style)Application.Current.Resources[typeof(TextBlock)];
            var newStyleTextBlock = new Style(typeof(TextBlock), styleTextBlock);
            newStyleTextBlock.Setters.Add(new Setter(Label.ForegroundProperty, textColor));
            Application.Current.Resources[typeof(TextBlock)] = newStyleTextBlock;
        }
        private void Button_Click_Logout(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            Close();
        }
        private void Button_Click_Seller(object sender, RoutedEventArgs e)
        {
            var productAndCountInSaller = new Dictionary<string, int>();
            foreach (var prod in ProductsData.Products)
            {
                if (prod.InSeller > 0)
                    productAndCountInSaller.Add(prod.Name, prod.InSeller);
                prod.InSeller = 0;
            }
            Sale sale = new Sale()
            {
                client = textBoxClient.Text.Trim(),
                date = DateTime.Now,
                salleNameAndCount = productAndCountInSaller,
            };
            SellerData.Sales.Add(sale);
            ProductControl.AllUpdateDataUI();
            UpdateButtonSeller();
        }
        void UpdateButtonSeller()
        {
            int countInSeller = ProductsData.Products.Sum(p =>  p.InSeller);
            buttonSeller.IsEnabled = textBoxClient.Text.Trim().Length > 0 && countInSeller > 0;
        }
        private void buttonSeller_MouseEnter(object sender, MouseEventArgs e)
        {
            UpdateButtonSeller();
        }
    }
}