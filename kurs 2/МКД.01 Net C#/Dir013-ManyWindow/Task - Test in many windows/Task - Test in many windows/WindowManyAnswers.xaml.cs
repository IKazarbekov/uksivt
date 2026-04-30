using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Task___Test_in_many_windows
{
    /// <summary>
    /// Логика взаимодействия для WindowManyAnswers.xaml
    /// </summary>
    public partial class WindowManyAnswers : Window
    {
        private Data.TestClose testCloseUser;
        public CheckBox[] checkBoxs;
        public WindowManyAnswers(bool autoInit = true)
        {
            InitializeComponent();

            if (autoInit)
            {
                checkBoxs = new CheckBox[]{
                    checkBoxAnswerFirst,
                    checkBoxAnswerSecond,
                    checkBoxAnswerThree,
                    checkBoxAnswerFour
                };
                // select test
                Random random = new Random();
                Data.TestManyAnswers[] tests = Data.testsManyAnswers;
                Data.TestManyAnswers test;
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
                int countAnswer = 4;
                if (test.ThreeAnswer == null)
                {
                    countAnswer = 2;
                    checkBoxAnswerThree.Visibility = Visibility.Collapsed;
                    checkBoxAnswerFour.Visibility = Visibility.Collapsed;
                }
                else if (test.FourAnswer == null)
                {
                    countAnswer = 3;
                    checkBoxAnswerFour.Visibility = Visibility.Collapsed;
                }
                string[] answers = { test.FirstAnswer, test.SecondAnswer, test.ThreeAnswer, test.FourAnswer };
                List<int> listCloseCheckBoxes = new List<int>();
                for (int i = 0; i < countAnswer; i++)
                {
                    int index = random.Next(countAnswer);
                    if (listCloseCheckBoxes.Contains(index))
                    {
                        i--;
                    }
                    else
                    {
                        checkBoxs[index].Content = answers[i];
                        listCloseCheckBoxes.Add(index);
                    }
                }
            }


        }

        private void buttonFront_Click(object sender, RoutedEventArgs e)
        {
            testCloseUser.UserAnswer = "";
            if (checkBoxAnswerFirst.IsChecked.Value)
                testCloseUser.UserAnswer += checkBoxAnswerFirst.Content.ToString() + "_";
            if (checkBoxAnswerSecond.IsChecked.Value)
                testCloseUser.UserAnswer += checkBoxAnswerSecond.Content.ToString() + "_";
            if (checkBoxAnswerThree.IsChecked.Value)
                testCloseUser.UserAnswer += checkBoxAnswerThree.Content.ToString() + "_";
            if (checkBoxAnswerFour.IsChecked.Value)
                testCloseUser.UserAnswer += checkBoxAnswerFour.Content.ToString() + "_";
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
