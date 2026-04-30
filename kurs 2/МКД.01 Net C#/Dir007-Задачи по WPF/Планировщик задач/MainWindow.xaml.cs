using System;
using System.Collections.Generic;
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

namespace Планировщик_задач
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        class Work
        {
            public enum Type
            {
                Work,
                Study,
                House,
                Other
            }
            private string text;
            private Type type;

            public Work(string text, Type type)
            {
                this.text = text;
                this.type = type;
            }
        }

        List<Work> works = new List<Work>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_Create_Work(object sender, RoutedEventArgs e)
        {
            string text = TextBoxWork.Text;
            string strType = ComboBoxWork.Text;
            Work.Type type = Work.Type.Other;
            switch (strType)
            {
                case "Работа":
                    type = Work.Type.Work;
                    break;
                case "Учёба":
                    type = Work.Type.Study;
                    break;
                case "Дом":
                    type = Work.Type.House;
                    break;
            }
            Work work = new Work(text, type);
            works.Add(work);
            TextBoxWork.Text = "";
        }
    }
}
