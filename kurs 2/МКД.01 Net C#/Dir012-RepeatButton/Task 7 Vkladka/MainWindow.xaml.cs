using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

namespace Task_7_Vkladka
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<string> languages = new ObservableCollection<string>()
        {
            "Русский","Английский","Немецкий"
        };

        public const string FILENAME = "save.bin";
        public MainWindow()
        {
            InitializeComponent();
            comboBoxLanguages.ItemsSource = languages;



        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateLanguage(comboBoxLanguages.SelectedIndex);
        }

        public void UpdateTopic(bool IsNight)
        {
            if (IsNight == true)
            {
                Resources["CurrentBackground"] = Resources["NightBackground"];
                Resources["CurrentForeground"] = Resources["NightForeground"];
            }
            else
            {
                Resources["CurrentBackground"] = Resources["LightBackground"];
                Resources["CurrentForeground"] = Resources["LightForeground"];
            }
        }

        public void UpdateLanguage(int languageIndex) {
            comboBoxLanguages.SelectionChanged -= ComboBox_SelectionChanged;
            switch (languageIndex)
            {
                case 0: // Русский
                    languages[0] = "Русский";
                    languages[1] = "Английский";
                    languages[2] = "Немецкий";
                    tabItemMain.Header = "Основные";
                    tabItemView.Header = "Вид";
                    tabItemAdditionlly.Header = "Дополнительно";
                    textBlockLanguage.Text = "Язык";
                    checkBoxAutoLoad.Content = "Автозагрузка";
                    textBlockTopic.Text = "Тема";
                    radioButtonLight.Content = "Светлая";
                    radioButtonNight.Content = "Тёмная";
                    radioButtonSystem.Content = "Системная";
                    textBlockFontSize.Text = "Размер шрифта";
                    checkBoxModeDeveloper.Content = "Режим разработчика";
                    textBlockLevelLog.Text = "Уровень логирования";
                    ComboBoxItemError.Content = "Ошибки";
                    ComboBoxItemWarning.Content = "Предупреждения";
                    ComboBoxItemInformation.Content = "Информация";

                    comboBoxLanguages.SelectedIndex = 0;
                    break;

                case 1: // Английский
                    languages[0] = "Russian";
                    languages[1] = "English";
                    languages[2] = "German";
                    tabItemMain.Header = "General";
                    tabItemView.Header = "View";
                    tabItemAdditionlly.Header = "Additional";
                    textBlockLanguage.Text = "Language";
                    checkBoxAutoLoad.Content = "Auto startup";
                    textBlockTopic.Text = "Theme";
                    radioButtonLight.Content = "Light";
                    radioButtonNight.Content = "Dark";
                    radioButtonSystem.Content = "System";
                    textBlockFontSize.Text = "Font size";
                    checkBoxModeDeveloper.Content = "Developer mode";
                    textBlockLevelLog.Text = "Logging level";
                    ComboBoxItemError.Content = "Errors";
                    ComboBoxItemWarning.Content = "Warnings";

                    comboBoxLanguages.SelectedIndex = 1;
                    break;

                case 2: // Немецкий
                    languages[0] = "Russisch";
                    languages[1] = "Englisch";
                    languages[2] = "Deutsch";
                    tabItemMain.Header = "Haupt";
                    tabItemView.Header = "Ansicht";
                    tabItemAdditionlly.Header = "Zusätzlich";
                    textBlockLanguage.Text = "Sprache";
                    checkBoxAutoLoad.Content = "Autostart";
                    textBlockTopic.Text = "Thema";
                    radioButtonLight.Content = "Hell";
                    radioButtonNight.Content = "Dunkel";
                    radioButtonSystem.Content = "System";
                    textBlockFontSize.Text = "Schriftgröße";
                    checkBoxModeDeveloper.Content = "Entwicklermodus";
                    textBlockLevelLog.Text = "Protokollierungsebene";
                    ComboBoxItemError.Content = "Fehler";
                    ComboBoxItemWarning.Content = "Warnungen";
                    ComboBoxItemInformation.Content = "Information";

                    comboBoxLanguages.SelectedIndex = 2;
                    break;
            }
            comboBoxLanguages.SelectionChanged += ComboBox_SelectionChanged;
        }

        private void radioButtonLight_Checked(object sender, RoutedEventArgs e)
        {
            UpdateTopic(false);
        }

        private void radioButtonNight_Checked(object sender, RoutedEventArgs e)
        {
            UpdateTopic(true);
        }

        private void sliderFontSize_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            double fontSize = sliderFontSize.Value;
            Resources["CurrentFontSize"] = fontSize;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            using(FileStream fs = new FileStream(FILENAME, FileMode.Create))
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                // language
                int index = comboBoxLanguages.SelectedIndex;
                bw.Write(index);
                // Auto Load
                bw.Write(checkBoxAutoLoad.IsChecked.Value);
                // Topic
                bool isNight = radioButtonNight.IsChecked.Value;
                bw.Write(isNight);
                // Font Size
                double fontSize = sliderFontSize.Value;
                bw.Write(fontSize);
                // Mode developer
                bool isDeveloper = checkBoxModeDeveloper.IsChecked.Value;
                bw.Write(isDeveloper);
                // Level log
                int level = comboBoxLog.SelectedIndex;
                bw.Write(level);
            }
            Close();
        }

        private void checkBoxModeDeveloper_Checked(object sender, RoutedEventArgs e)
        {
            UpdateModeDeveloper(checkBoxModeDeveloper.IsChecked.Value);
        }

        public void UpdateModeDeveloper(bool b)
        {
            comboBoxLog.IsEnabled = b;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (FileStream fs = new FileStream(FILENAME, FileMode.Open))
            using (BinaryReader br = new BinaryReader(fs))
            {
                // language
                int index = br.ReadInt32();
                comboBoxLanguages.SelectedIndex = index;
                // Auto Load
                bool isAutoLoad = br.ReadBoolean();
                checkBoxAutoLoad.IsChecked = isAutoLoad;
                // Topic
                bool isNight = br.ReadBoolean();
                radioButtonNight.IsChecked = isNight;
                // Font Size
                double fontSize = br.ReadDouble();
                sliderFontSize.Value = fontSize;
                // Mode developer
                bool isDeveloper = br.ReadBoolean();
                checkBoxModeDeveloper.IsChecked = isDeveloper;
                // Level log
                int level = br.ReadInt32();
                comboBoxLog.SelectedIndex = level;
            }
        }
    }
}
