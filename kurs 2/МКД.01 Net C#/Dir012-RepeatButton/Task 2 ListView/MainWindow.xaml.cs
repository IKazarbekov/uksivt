using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace Task_2_ListView
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = products;
        }

        class Product
        {
            public string Name { get; set; }
            public string Category { get; set; }
            public int Price { get; set; }

            public string ToString()
            {
                return $"{Name}, caategory - {Category}, price:{Price}";
            }
        }

        ObservableCollection<Product> products = new ObservableCollection<Product>()
        {
                new Product()
                {
                    Name = "Ноутбук",
                    Category = "Электроника",
                    Price = 75000
                },
                new Product()
                {
                    Name = "Смартфон",
                    Category = "Электроника",
                    Price = 45000
                },
                new Product()
                {
                    Name = "Книга",
                    Category = "Литература",
                    Price = 800
                },
                new Product()
                {
                    Name = "Футболка",
                    Category = "Одежда",
                    Price = 1500
                },
                new Product()
                {
                    Name = "Кроссовки",
                    Category = "Обувь",
                    Price = 5000
                },
                new Product()
                {
                    Name = "Микроволновка",
                    Category = "Бытовая техника",
                    Price = 12000
                },
                new Product()
                {
                    Name = "Часы",
                    Category = "Аксессуары",
                    Price = 3000
                },
                new Product()
                {
                    Name = "Наушники",
                    Category = "Электроника",
                    Price = 3500
                },
                new Product()
                {
                    Name = "Рюкзак",
                    Category = "Аксессуары",
                    Price = 2500
                },
                new Product()
                {
                    Name = "Планшет",
                    Category = "Электроника",
                    Price = 30000
                }
        };
    }
}
