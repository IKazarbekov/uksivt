using System;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace Task
{
    internal class Shoes
    {
        [JsonIgnore]
        public static ObservableCollection<Shoes> shoesies = new ObservableCollection<Shoes>();
        public string Description { get; set; }
        [JsonIgnore]
        public string DescriptionStr
        {
            get
            {
                return $"Описание: {Description}";
            }
        }
        public string Name { get; set; }
        public string Category { get; set; }
        public string NameAndCategory
        {
            get
            {
                return Name + "|" + Category;
            }
        }

        public string PathImage { get; set; }
        [JsonIgnore]
        public BitmapImage Image
        {
            get
            {
                return new BitmapImage(new Uri(PathImage, UriKind.Relative));
            }
        }
        public int Price { get; set; }
        [JsonIgnore]
        public string PriceStr
        {
            get
            {
                return $"Цена: {Price}";
            }
        }
        public int Count { get; set; }
        [JsonIgnore]
        public string CountStr
        {
            get
            {
                return $"Количество на складе: {Count}";
            }
        }
        public int Discount { get; set; }
        [JsonIgnore]
        public string DiscountStr
        {
            get
            {
                return $"Скидка: {Discount}";
            }
        }
        [JsonIgnore]
        public SolidColorBrush Color
        {
            get
            {
                if (Discount > 10)
                    return Brushes.Red;
                else
                    return Brushes.White;
            }
        }
    }
}
