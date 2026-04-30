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

namespace Task_8_ListBox
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public CollectionViewSource collectionViewSource;
        public ObservableCollection<Product> products = new ObservableCollection<Product>()
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
            collectionViewSource = (CollectionViewSource)Resources["viewProducts"];
        }

        public class Product
        {
            public string Name { get; set; }
            public int Price { get; set; }
            public string Category { get; set; }
        }


        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView listView = sender as ListView;

            int summa = 0;

            foreach (Product product in listView.SelectedItems)
            {
                summa += product.Price;
            }

            textBlockPrice.Text = $"Итог: {summa} рублей";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder strBuild = new StringBuilder("Cписок товаров:\n");
            foreach (Product product in listView.SelectedItems)
            {
                strBuild.Append($"{product.Name}\n");
            }
            MessageBox.Show(strBuild.ToString());
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            bool isCheck = checkBox.IsChecked.Value;

            if (isCheck == true)
                collectionViewSource.View.Filter = pr =>
                {
                    Product product = pr as Product;
                    return product.Category == "Электроника";
                };
            else
                collectionViewSource.View.Filter = null;
        }
    }
}
