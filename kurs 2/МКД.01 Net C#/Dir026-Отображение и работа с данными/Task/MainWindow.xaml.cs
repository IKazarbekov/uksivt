using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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

namespace Task
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            /*
            Shoes.shoesies.Add(new Shoes()
            {
                Name = "Коричок",
                Category = "Туфли",
                Description = "Обувь моего соседа, который постоянно его зашивает",
                Price=15,
                Count = 14,
                Discount = 5,
                PathImage="/images/1.png"
            });
            Shoes.shoesies.Add(new Shoes()
            {
                Name = "Дличок",
                Category = "Странная обувь",
                Description = "Самая не удобная обувь на свете",
                Price = 9999,
                Count = 9999,
                Discount = 0,
                PathImage = "/images/2.png"
            });
            Shoes.shoesies.Add(new Shoes()
            {
                Name = "Пантера",
                Category = "Туфли",
                Description = "Хочешь быть успешным ? Навервай эту обувь и всё не получится !",
                Price = 60,
                Count = 24,
                Discount = 80,
                PathImage = "/images/3.png"
            });
            string json = JsonSerializer.Serialize(Shoes.shoesies, new JsonSerializerOptions()
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                IgnoreReadOnlyFields = true
            });

            File.WriteAllText("file.json", json);
            */
            string json1 = File.ReadAllText("file.json");
            Shoes.shoesies = JsonSerializer.Deserialize<ObservableCollection<Shoes>>(json1);

            DataContext = Shoes.shoesies;
        }
    }
}
