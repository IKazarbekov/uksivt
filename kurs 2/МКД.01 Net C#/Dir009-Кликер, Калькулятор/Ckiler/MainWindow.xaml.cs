using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Ckiler
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        int level = 0;
        int countNoteBook = 0;
        int ball = 0;
        int levelUpgrade = 0;
        int priseIpdate = 120;
        int status = 0;

        bool flag = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        // Main button click
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            switch (level)
            {
                case 1:
                    MainLabel.Content = "Помню что сумка была полной когда выходил";
                    MainButton.Content = "Далее";
                    break;
                case 2:
                    MainLabel.Content = "У меня сумка открысь по пути в УКСИВТ";
                    break;
                case 3:
                    MainLabel.Content = "Мои тетради падали с моей сумки, пока я шёл сюда";
                    MainButton.Content = "Далее";
                    break;
                case 4:
                    MainLabel.Content = "Что делать?";
                    MainButton.Content = "Найти тетради";
                    MainButton2.Visibility = Visibility.Visible;
                    break;
                case 5:
                    UpdateButtons("Итак, где то в этом классе они точно должны быть", "Начать искать");
                    break;
                case 6:
                    UpdateButtons("Найти тетрадь", "Найти", "Выбросить");
                    UpdateUI();
                    countNoteBook += 1 + levelUpgrade;
                    level--;

                    if (countNoteBook > 35)
                    {
                        MessageBox.Show("Возникла мысль!");
                        UpdateButtons("Хм собрал уже более 35 тетрадей, но у меня было их где-то 695 штук", "Дальше искать", "Показать учителю");
                        level++;
                        UpdateUI();
                    }
                    break;
                case 7:
                    UpdateButtons("Найти тетрадь", "Найти", "Выбросить");
                    flag = true;
                    level++;
                    break;
                case 9:
                    UpdateUI();
                    countNoteBook += 1 + levelUpgrade;
                    level--;

                    if (countNoteBook > 100)
                    {
                        MessageBox.Show("Возникла мысль!");
                        UpdateButtons("Уффф я УСТАЛ!", "Дальше");
                        level++;
                        UpdateUI();
                    }
                    break;
                case 10:
                    UpdateButtons("Уффф я УСТАЛ ИСКАТЬ!", "Дальше");
                    break;
                case 11:
                    UpdateButtons("Студент: Эй! Давай помогу, но только я тоже забыл тетради", "Дальше");
                    break;
                case 12:
                    UpdateButtons("Студент: И учителю это не понравится", "Дальше");
                    break;
                case 13:
                    UpdateButtons("Студент: Если дашь мне 120 тетрадей, то я помогу тебе", "Дальше");
                    break;
                case 14:
                    UpdateButtons("Найти тетрадь", "Найти", "Выбросить");
                    UniformGridButtons.Visibility = Visibility.Visible;
                    break;
                case 15:
                    UpdateUI();
                    countNoteBook += 1 + levelUpgrade;
                    level--;

                    if (countNoteBook > 695)
                    {
                        MessageBox.Show("Возникла мысль!");
                        UpdateButtons("Ураа я собрал все тетради!", "Дальше");
                        level++;
                        UpdateUI();
                    }
                    break;
                case 16:
                    UpdateButtons("Ураа я собрал все тетради! Я КРУТОЙ!", "Дальше");
                    ImageBrushWindow.ImageSource = new BitmapImage(new Uri($"pack://application:,,,/resourses/крутой.jpg", UriKind.Absolute));
                    break;
                case 17:
                    UpdateButtons("И я не получу 2!", "Дальше");
                    break;
                case 18:
                    UpdateButtons("Мы весь симместр писали 695 тетрадей!", "Дальше");
                    break;
                case 19:
                    UpdateButtons("О том как правильно рисовать!", "Дальше");
                    break;
                case 20:
                    UpdateButtons("И хотя за весь симметр мы ни разу не рисовали...", "Дальше");
                    break;
                case 21:
                    UpdateButtons("Я заслужил 5 за симметр", "Дальше");
                    break;
                case 22:
                    ImageBrushWindow.ImageSource = new BitmapImage(new Uri($"pack://application:,,,/resourses/clearкласс.jpg", UriKind.Absolute));
                    UpdateButtons("...", "Дальше");
                    break;
                case 23:
                    UpdateButtons("А где мои одногруппники и учитель?", "Дальше");
                    break;
                case 24:
                    UpdateButtons("Студент: Пара закончилась", "Дальше");
                    break;
                case 25:
                    UpdateButtons("Студент: И так как ты опоздал", "Дальше");
                    break;
                case 26:
                    UpdateButtons("Студент: Тебе поставили 2", "Дальше");
                    break;
                case 27:
                    Close();
                    break;
            }

            level++;
        }

        // Main button 2 click
        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            switch (level)
            {
                case 5:
                    MainButton.Visibility = Visibility.Collapsed;
                    MainLabel.Content = "Учитель: 2 тебе !";
                    MainButton2.Content = "Вы проиграли";
                    Close();
                    break;
                case 6:
                    UpdateButtons("Найти тетрадь", "Найти", "Выбросить");
                    UpdateUI();
                    level -= 1;
                    if (countNoteBook > 0)
                        countNoteBook--;
                    break;
                case 7:
                    UpdateButtons("И это всё! А где остальные 560 тетрадей! Два тебе!", null, "Вы проиграли");
                    break;
                case 8:
                    Close();
                    break;
                case 9:
                 
                        UpdateUI();
                        level -= 1;
                        if (countNoteBook > 0)
                            countNoteBook--;
                        break;
                    
 

            }

            level++;
        }

        private void UpdateButtons(string textLabel, string textButton1, string textButton2 = null)
        {
            if (textButton2 == null)
                MainButton2.Visibility = Visibility.Collapsed;
            else
            {
                MainButton2.Visibility = Visibility.Visible;
                MainButton2.Content = textButton2;
            }

            if (textButton1 == null)
                MainButton.Visibility = Visibility.Collapsed;
            else
            {
                MainButton.Visibility = Visibility.Visible;
                MainButton.Content = textButton1;
            }
            MainLabel.Content = textLabel;

        }

        private void UpdateUI()
        {
            LabelStatus.Visibility = Visibility.Visible;
            LabelStatus.Text = $"Тетрадей:{countNoteBook}, Помощников:{levelUpgrade}, Цена помощника:{priseIpdate}, level:{level}";

            if (countNoteBook >= priseIpdate)
                ButtonAddStudent.IsEnabled = true;
            else
                ButtonAddStudent.IsEnabled = false;

            if (countNoteBook > 0)
            {
                ButtonDrop.IsEnabled = true;
                MainButton2.IsEnabled = true;
            }
            else
            {
                ButtonDrop.IsEnabled = false;
                MainButton2.IsEnabled = false;
            }
        }

        // Clear count notebooks
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            countNoteBook = 0;
            UpdateUI();
            UpdateUI();
        }

        // Upgrate
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (countNoteBook > priseIpdate)
            {
                levelUpgrade++;
                countNoteBook -= priseIpdate;
                priseIpdate *= (int)1.3;
            }
            UpdateUI();
        }

        // Change location
        string[] paths = { "/класс.jpg", "/коридор.jpg", "/улица.jpg" };
        int index = 0;
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            index++;
            if (index == paths.Length)
            {
                index = 0;
            }

            ImageBrushWindow.ImageSource = new BitmapImage(new Uri($"pack://application:,,,/resourses{paths[index]}", UriKind.Absolute));
        }

        // Change color
        int indexColor = 0;
        SolidColorBrush[] colors = { Brushes.Red, Brushes.Purple, Brushes.White, Brushes.Black };
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            indexColor++;
            if (indexColor == colors.Length)
                indexColor = 0;
            LabelStatus.Foreground = colors[indexColor];
        }
    }
}
