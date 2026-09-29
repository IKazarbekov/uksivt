using System;

namespace ShopTask.models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Переопределяем ToString, чтобы компилятор без проблем читал имя категории
        public override string ToString()
        {
            return Name ?? string.Empty;
        }

        // Автоматическое приведение к string для удобства работы в LINQ-запросах
        public static implicit operator string(Category category)
        {
            return category?.Name ?? string.Empty;
        }
    }
}