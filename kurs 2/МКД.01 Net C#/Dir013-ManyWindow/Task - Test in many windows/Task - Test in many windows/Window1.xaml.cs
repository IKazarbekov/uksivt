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
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private Data.TestClose testCloseUser;
        public Window1(bool autoInit = true)
        {
            InitializeComponent();

            if (autoInit)
            {
                // select test
                Random random = new Random();
                Data.TestOneAnswer[] tests = Data.testsOneAnswers;
                Data.TestOneAnswer test;
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

                // write answers
                int indexTrue = random.Next(0, test.CountAnswer);
                if (test.FourAnswer == null)
                {
                    radioButtonAnswerFour.Visibility = Visibility.Collapsed;
                    indexTrue = random.Next(0, test.CountAnswer - 1);
                }
                if (test.ThreeAnswer == null)
                {
                    radioButtonAnswerThree.Visibility = Visibility.Collapsed;
                    indexTrue = random.Next(0, test.CountAnswer - 2);
                }
                switch (indexTrue)
                {
                    case 0:
                        radioButtonAnswerFirst.Content = test.TrueAnswer;
                        radioButtonAnswerSecond.Content = test.SecondAnswer;
                        radioButtonAnswerThree.Content = test.ThreeAnswer;
                        radioButtonAnswerFour.Content = test.FourAnswer;
                        break;
                    case 1:
                        radioButtonAnswerFirst.Content = test.SecondAnswer;
                        radioButtonAnswerSecond.Content = test.TrueAnswer;
                        radioButtonAnswerThree.Content = test.ThreeAnswer;
                        radioButtonAnswerFour.Content = test.FourAnswer;
                        break;
                    case 2:
                        radioButtonAnswerFirst.Content = test.ThreeAnswer;
                        radioButtonAnswerSecond.Content = test.SecondAnswer;
                        radioButtonAnswerThree.Content = test.TrueAnswer;
                        radioButtonAnswerFour.Content = test.FourAnswer;
                        break;
                    case 3:
                        radioButtonAnswerFirst.Content = test.ThreeAnswer;
                        radioButtonAnswerSecond.Content = test.SecondAnswer;
                        radioButtonAnswerThree.Content = test.FourAnswer;
                        radioButtonAnswerFour.Content = test.TrueAnswer;
                        break;
                }
            }
            
            
        }

        private void buttonFront_Click(object sender, RoutedEventArgs e)
        {
            if (radioButtonAnswerFirst.IsChecked.Value)
                testCloseUser.UserAnswer = radioButtonAnswerFirst.Content.ToString();
            if (radioButtonAnswerSecond.IsChecked.Value)
                testCloseUser.UserAnswer = radioButtonAnswerSecond.Content.ToString();
            if (radioButtonAnswerThree.IsChecked.Value)
                testCloseUser.UserAnswer = radioButtonAnswerThree.Content.ToString();
            if (radioButtonAnswerFour.IsChecked.Value)
                testCloseUser.UserAnswer = radioButtonAnswerFour.Content.ToString();
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
