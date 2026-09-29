using System;
using System.IO;
using System.Text.Json.Serialization;
using System.Windows.Media.Imaging;

namespace ShopTask.models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Category Category { get; set; }
        public Status Status { get; set; }
        public int Price { get; set; }
        public int InSeller { get; set; }
        public int Count { get; set; }
        public string ImagePath { get; set; }
        [JsonIgnore]
        public BitmapImage Image
        {
            get
            {
                if (ImagePath == null)
                    return new BitmapImage(new System.Uri("/prog_images/dafault_product.png", System.UriKind.Relative));
                return new BitmapImage(new System.Uri(Path.GetFullPath(ImagePath), System.UriKind.Absolute));
            }
        }
        [JsonIgnore]
        public string PriceStr
        {
            get { return $"{Price} рублей"; }
        }
        [JsonIgnore]
        public Array CategoryValues
        {
            get
            {
                return Enum.GetValues(typeof(Category));
            }
        }

    }
}
