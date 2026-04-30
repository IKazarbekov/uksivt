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
using System.Windows.Shapes;

namespace Task___Test_in_many_windows
{
    /// <summary>
    /// Логика взаимодействия для WindowStringAnswer.xaml
    /// </summary>
    public partial class WindowStringAnswer : Window
    {
        private Data.TestClose testCloseUser;
        public WindowStringAnswer(bool autoInit = true)
        {
            InitializeComponent();
            if (autoInit)
            {
                // select test
                Random random = new Random();
                Data.TestString[] tests = Data.testsString;
                Data.TestString test;
                while (true)
                {
                    int index = random.Next(0, tests.Length);
                    test = tests[index];
                    if (!Data.testsClose.Contains(test))
                        break;
                }
                Data.testsClose.Add(test);
                testCloseUser = new Data.TestClose() { test = test };
                Data.testClosesUser.Add(testCloseUser);

                // write name and question
                textBlockName.Text = Data.currentName;
                textBlockQuestion.Text = $"Вопрос #{Data.questionNumber}: {test.Question}";
            }
            

        }

        private void buttonFront_Click(object sender, RoutedEventArgs e)
        {
            testCloseUser.UserAnswer = textBoxAnswer.Text;
            Utills.NextWindow();
            Close();
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            Utills.BackWindow();
            Close();
        }
    }
}
