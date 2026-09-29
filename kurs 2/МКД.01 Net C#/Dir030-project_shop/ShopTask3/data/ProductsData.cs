using System.Collections.Generic;
using System.Collections.ObjectModel;
using ShopTask.models;

namespace ShopTask.data
{
    public static class ProductsData
    {
        // Списки, к которым обращается интерфейс
        public static ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();
        public static List<Category> Categories { get; set; } = new List<Category>();

        static ProductsData()
        {
            // Инициализируем базовые категории
            var electro = new Category { Id = 1, Name = "Электроника" };
            var food = new Category { Id = 2, Name = "Продукты питания" };
            var Clothes = new Category { Id = 3, Name = "Одежда" };

            Categories.Add(electro);
            Categories.Add(food);
            Categories.Add(Clothes);

            // Наполняем тестовыми товарами для витрины
            Products.Add(new Product { Id = 1, Name = "Смартфон NextGen", Description = "Флагманский телефон", Price = 49999, Count = 5, Category = electro });
            Products.Add(new Product { Id = 2, Name = "Ноутбук Maibenben PRO", Description = "Мощная рабочая станция", Price = 75000, Count = 3, Category = electro });
            Products.Add(new Product { Id = 3, Name = "Шоколад Молочный", Description = "Очень сладкий", Price = 120, Count = 50, Category = food });
            Products.Add(new Product { Id = 4, Name = "Худи Оверсайз", Description = "Теплая брендовая кофта", Price = 3500, Count = 12, Category = Clothes });
        }
    }
}