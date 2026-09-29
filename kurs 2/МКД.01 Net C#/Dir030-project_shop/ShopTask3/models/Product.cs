using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace ShopTask.models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }

        // Связь с типом Category
        public Category Category { get; set; }
        public string ImagePath { get; set; }

        // Динамическое свойство для WPF, возвращающее готовый BitmapImage
        public BitmapImage Image
        {
            get
            {
                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();

                // Проверяем, существует ли кастомный файл изображения
                if (!string.IsNullOrEmpty(ImagePath) && File.Exists(ImagePath))
                {
                    bmp.UriSource = new Uri(Path.GetFullPath(ImagePath));
                }
                else
                {
                    // Если картинки нет — ставим системную заглушку (убедись, что у тебя есть файл или относительный путь)
                    bmp.UriSource = new Uri("images/default.png", UriKind.Relative);
                }

                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.EndInit();
                return bmp;
            }
        }

        // Коллекция возможных значений категорий для ComboBox внутри DataGrid
        public System.Collections.Generic.List<Category> CategoryValues => data.ProductsData.Categories;
    }
}