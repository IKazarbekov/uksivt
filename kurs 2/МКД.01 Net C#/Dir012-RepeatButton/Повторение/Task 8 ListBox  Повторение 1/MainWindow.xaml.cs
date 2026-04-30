using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Task_8_ListBox__Повторение_1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class Product
        {
            public string Name { get; set; }
            public string Category { get; set; }
            public int Price { get; set; }
        }

        public ObservableCollection<Product> products = new ObservableCollection<Product>
        {
            new Product { Name = "Ноутбук", Price = 75000, Category = "Электроника" },
            new Product { Name = "Смартфон", Price = 45000, Category = "Электроника" },
            new Product { Name = "Наушники", Price = 5000, Category = "Электроника" },
            new Product { Name = "Планшет", Price = 35000, Category = "Электроника" },
            new Product { Name = "Умные часы", Price = 15000, Category = "Электроника" },
            new Product { Name = "Колонка", Price = 3000, Category = "Электроника" },
            new Product { Name = "Футболка", Price = 1500, Category = "Одежда" },
            new Product { Name = "Джинсы", Price = 4000, Category = "Одежда" },
            new Product { Name = "Куртка", Price = 8000, Category = "Одежда" },
            new Product { Name = "Роман", Price = 500, Category = "Книги" },
            new Product { Name = "Учебник C#", Price = 2000, Category = "Книги" },
            new Product { Name = "Энциклопедия", Price = 2500, Category = "Книги" },
            new Product { Name = "Хлеб", Price = 50, Category = "Продукты" },
            new Product { Name = "Молоко", Price = 80, Category = "Продукты" },
            new Product { Name = "Сыр", Price = 300, Category = "Продукты" }
        };

        public MainWindow()
        {
            InitializeComponent();
            DataContext = products;
        }





        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;

            bool isCheck = checkBox.IsChecked.Value;

            if (isCheck == true)
                ((CollectionViewSource)Resources["colViewSource"]).View.Filter = item =>
                {
                    Product product = item as Product;
                    return product.Category == "Электроника";
                };
            else
                ((CollectionViewSource)Resources["colViewSource"]).View.Filter = null;
        }
    }
}
