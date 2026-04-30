using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Task___Test_in_many_windows
{
    /// <summary>
    /// Логика взаимодействия для WindowResult.xaml
    /// </summary>
    public partial class WindowResult : Window
    {
        public WindowResult(bool autoInit = true)
        {
            InitializeComponent();

            if (autoInit)
            {
                // Read file
                if (File.Exists("file.bin"))
                    using (FileStream fs = new FileStream("file.bin", FileMode.Open))
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        Data.statistic.Clear();
                        int count = br.ReadInt32();
                        for (int i = 0; i < count; i++)
                        {
                            string name = br.ReadString();
                            int ball = br.ReadInt32();
                            Data.statistic.Add(name, ball);
                        }
                    }

                // Write data
                textBlockName.Text = Data.currentName;
                textBlockResult.Text = $"Ваш результат: {Data.ResultPoints}";
                dataGridStatistic.ItemsSource = Data.statistic;
            }


        }

        private void Window_Closed(object sender, EventArgs e)
        {
            // Save statisctics
            var dict = Data.statistic;
            using (FileStream fs = new FileStream("file.bin", FileMode.Create))
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                bw.Write(dict.Count);
                foreach(KeyValuePair<String, int> pair in dict)
                {
                    bw.Write(pair.Key);
                    bw.Write(pair.Value);
                }
            }

            // Open start window
            new MainWindow().Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
