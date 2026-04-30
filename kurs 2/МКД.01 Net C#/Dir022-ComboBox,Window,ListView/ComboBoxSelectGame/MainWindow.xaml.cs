using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ComboBoxSelectGame
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Game> gamesPastWar = new List<Game>()
        {
            new Game()
            {
                Name = "Call of Duty",
                Description = "Сюжет: Вторая мировая война, кампании США, Британии и СССР. Геймплей: классический шутер от первого лица, линейные миссии, реалистичное оружие.",
                Raiting = 4,
            },
            new Game()
            {
                Name = "Call of Duty 2",
                Description = "Сюжет: Продолжение ВМВ, битвы в Северной Африке и Европе. Геймплей: улучшенная графика, перезаряжаемое здоровье, более масштабные сражения.",
                Raiting = 5
            },

        };
        new List<Game> gamesNow = new List<Game>() {
            new Game()
            {
                Name = "Call of Duty 4: Modern Warfare",
                Description = "Сюжет: Современная война с террористами, SAS и морпехи США. Геймплей: революционная смена эпохи, кастомное оружие, культовая миссия 'All Ghillied Up'.",
                Raiting = 5
            },
            new Game()
            {
                Name = "Call of Duty: Modern Warfare 2",
                Description = "Сюжет: Русские захватывают США, полковник Шепард предаёт отряд 141. Геймплей: скандальная миссия 'No Russian', спецназ, вариативное прохождение.",
                Raiting = 5
            },
           new Game()
            {
                Name = "Call of Duty: Modern Warfare 3",
                Description = "Сюжет: Мировая война, уничтожение Макарова, гибель Соупа. Геймплей: завершение трилогии, режим 'Выживание', эпичные бои по всему миру.",
                Raiting = 4
            },
        };
        new List<Game> gamesFuture = new List<Game>() {
            new Game()
            {
                Name = "Call of Duty: Black Ops",
                Description = "Сюжет: Холодная война, мозгопромывка, тайные операции, Виктор Резнов. Геймплей: запутанный сюжет с флешбэками, улучшенный зомби-режим.",
                Raiting = 5
            },

            new Game()
            {
                Name = "Call of Duty: Black Ops II",
                Description = "Сюжет: Две линии времени (1980-е и 2025 год), выбор влияет на концовку, злодей Менендес. Геймплей: ветвящийся сюжет, стратегические миссии со страйк-форс.",
                Raiting = 5
            },
        };
        public MainWindow()
        {
            InitializeComponent();
            gamesPastWar[0].Image = (BitmapImage)this.Resources["cod1"];
            gamesPastWar[1].Image = (BitmapImage)this.Resources["cod2"];
            gamesNow[0].Image = (BitmapImage)this.Resources["mv1"];
            gamesNow[1].Image = (BitmapImage)this.Resources["mv2"];
            gamesNow[2].Image = (BitmapImage)this.Resources["mv3"];
            gamesFuture[0].Image = (BitmapImage)this.Resources["bo1"];
            gamesFuture[1].Image = (BitmapImage)this.Resources["bo2"];
            comboBoxCategory.SelectedIndex = 0;
            comboBoxGame.SelectedIndex = 0;
        }
        private void comboBoxCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = comboBoxCategory.SelectedItem as ComboBoxItem;
            string tag = item.Tag.ToString();
            imageCategory.Source = (BitmapImage)this.Resources[tag];

            switch (tag)
            {
                case "past":
                    comboBoxGame.ItemsSource = gamesPastWar;
                    comboBoxGame.SelectedIndex = gamesPastWar.Count - 1;
                    break;
                case "now":
                    comboBoxGame.ItemsSource = gamesNow;
                    comboBoxGame.SelectedIndex = gamesNow.Count - 1;
                    break;
                case "future":
                    comboBoxGame.ItemsSource = gamesFuture;
                    comboBoxGame.SelectedIndex = gamesFuture.Count - 1;
                    break;
            }
        }
        class Game
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public int Raiting { get; set; }
            public string RaitingStr { get { return "Рейтинг: " + Raiting; }  }
            public BitmapImage Image { get; set; }
        }

        private void comboBoxGame_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Game game = (Game)comboBoxGame.SelectedItem;
            imageGame.Source = game?.Image;
        }
    }
}
