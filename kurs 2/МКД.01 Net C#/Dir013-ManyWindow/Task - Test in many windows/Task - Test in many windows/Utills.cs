using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Windows;
using System.Windows.Controls;

namespace Task___Test_in_many_windows
{
    internal static class Utills
    {
        private static Random random = new Random();
        public static void NextWindow()
        {

            if (Data.currentQuestionNumber >= Data.COUNT_QUESTION)
            {
                new WindowResult().Show();
            }
            else
            {
                Data.currentQuestionNumber++;
                // Если вопрос новый
                if (Data.questionNumber < Data.currentQuestionNumber)
                {
                    Data.questionNumber++;
                    int indexType = random.Next(3);
                    if (indexType == 0)
                        new Window1().Show();
                    else if (indexType == 1)
                        new WindowManyAnswers().Show();
                    else
                        new WindowStringAnswer().Show();
                }
                // Если вопрос уже был
                else
                {
                    Data.TestClose textUser = Data.testClosesUser[Data.currentQuestionNumber];
                    string answer = textUser.UserAnswer;
                    Data.Test test = textUser.test;
                    if (test is Data.TestString testString)
                    {
                        WindowStringAnswer windowString = new WindowStringAnswer(false);
                        windowString.textBoxAnswer.Text = answer;
                        windowString.textBlockQuestion.Text = testString.Question;
                    }
                    else if (test is Data.TestOneAnswer testOneAnswer)
                    {
                        Window1 window = new Window1(false);
                        // write name and question
                        window.textBlockName.Text = Data.currentName;
                        window.textBlockQuestion.Text = $"Вопрос #{Data.questionNumber}: {testOneAnswer.Question}";
                        // write answers
                        int indexTrue = random.Next(0, testOneAnswer.CountAnswer);
                        if (testOneAnswer.FourAnswer == null)
                        {
                            window.radioButtonAnswerFour.Visibility = Visibility.Collapsed;
                            indexTrue = random.Next(0, testOneAnswer.CountAnswer - 1);
                        }
                        if (testOneAnswer.ThreeAnswer == null)
                        {
                            window.radioButtonAnswerThree.Visibility = Visibility.Collapsed;
                            indexTrue = random.Next(0, testOneAnswer.CountAnswer - 2);
                        }
                        switch (indexTrue)
                        {
                            case 0:
                                window.radioButtonAnswerFirst.Content = testOneAnswer.TrueAnswer;
                                window.radioButtonAnswerSecond.Content = testOneAnswer.SecondAnswer;
                                window.radioButtonAnswerThree.Content = testOneAnswer.ThreeAnswer;
                                window.radioButtonAnswerFour.Content = testOneAnswer.FourAnswer;
                                break;
                            case 1:
                                window.radioButtonAnswerFirst.Content = testOneAnswer.SecondAnswer;
                                window.radioButtonAnswerSecond.Content = testOneAnswer.TrueAnswer;
                                window.radioButtonAnswerThree.Content = testOneAnswer.ThreeAnswer;
                                window.radioButtonAnswerFour.Content = testOneAnswer.FourAnswer;
                                break;
                            case 2:
                                window.radioButtonAnswerFirst.Content = testOneAnswer.ThreeAnswer;
                                window.radioButtonAnswerSecond.Content = testOneAnswer.SecondAnswer;
                                window.radioButtonAnswerThree.Content = testOneAnswer.TrueAnswer;
                                window.radioButtonAnswerFour.Content = testOneAnswer.FourAnswer;
                                break;
                            case 3:
                                window.radioButtonAnswerFirst.Content = testOneAnswer.ThreeAnswer;
                                window.radioButtonAnswerSecond.Content = testOneAnswer.SecondAnswer;
                                window.radioButtonAnswerThree.Content = testOneAnswer.FourAnswer;
                                window.radioButtonAnswerFour.Content = testOneAnswer.TrueAnswer;
                                break;
                        }
                    }
                    else if (test is Data.TestManyAnswers testManyAnswer)
                    {
                        WindowManyAnswers window = new WindowManyAnswers();
                        window.checkBoxs = new CheckBox[]{
                                window.checkBoxAnswerFirst,
                                window.checkBoxAnswerSecond,
                                window.checkBoxAnswerThree,
                                window.checkBoxAnswerFour
                            };

                        // write name and question
                        window.textBlockName.Text = Data.currentName;
                        window.textBlockQuestion.Text = $"Вопрос #{Data.questionNumber}: {testManyAnswer.Question}";

                        // write answers
                        int countAnswer = 4;
                        if (testManyAnswer.ThreeAnswer == null)
                        {
                            countAnswer = 2;
                            window.checkBoxAnswerThree.Visibility = Visibility.Collapsed;
                            window.checkBoxAnswerFour.Visibility = Visibility.Collapsed;
                        }
                        else if (testManyAnswer.FourAnswer == null)
                        {
                            countAnswer = 3;
                            window.checkBoxAnswerFour.Visibility = Visibility.Collapsed;
                        }
                        string[] answers = { testManyAnswer.FirstAnswer, testManyAnswer.SecondAnswer, testManyAnswer.ThreeAnswer, testManyAnswer.FourAnswer };
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
                                window.checkBoxs[index].Content = answers[i];
                                listCloseCheckBoxes.Add(index);
                            }
                        }
                    }
                }

            }

        }
        public static void BackWindow()
        {
            
            Data.currentQuestionNumber--;
            Data.TestClose textUser = Data.testClosesUser[Data.currentQuestionNumber];
            string answer = textUser.UserAnswer;
            Data.Test test = textUser.test;
            if (test is Data.TestString testString)
            {
                WindowStringAnswer windowString = new WindowStringAnswer(false);
                windowString.textBoxAnswer.Text = answer;
                windowString.textBlockQuestion.Text = testString.Question;
                windowString.Show();
            }
            else if (test is Data.TestOneAnswer testOneAnswer)
            {
                Window1 window = new Window1(false);
                // write name and question
                window.textBlockName.Text = Data.currentName;
                window.textBlockQuestion.Text = $"Вопрос #{Data.questionNumber}: {testOneAnswer.Question}";
                // write answers
                int indexTrue = random.Next(0, testOneAnswer.CountAnswer);
                if (testOneAnswer.FourAnswer == null)
                {
                    window.radioButtonAnswerFour.Visibility = Visibility.Collapsed;
                    indexTrue = random.Next(0, testOneAnswer.CountAnswer - 1);
                }
                if (testOneAnswer.ThreeAnswer == null)
                {
                    window.radioButtonAnswerThree.Visibility = Visibility.Collapsed;
                    indexTrue = random.Next(0, testOneAnswer.CountAnswer - 2);
                }
                switch (indexTrue)
                {
                    case 0:
                        window.radioButtonAnswerFirst.Content = testOneAnswer.TrueAnswer;
                        window.radioButtonAnswerSecond.Content = testOneAnswer.SecondAnswer;
                        window.radioButtonAnswerThree.Content = testOneAnswer.ThreeAnswer;
                        window.radioButtonAnswerFour.Content = testOneAnswer.FourAnswer;
                        break;
                    case 1:
                        window.radioButtonAnswerFirst.Content = testOneAnswer.SecondAnswer;
                        window.radioButtonAnswerSecond.Content = testOneAnswer.TrueAnswer;
                        window.radioButtonAnswerThree.Content = testOneAnswer.ThreeAnswer;
                        window.radioButtonAnswerFour.Content = testOneAnswer.FourAnswer;
                        break;
                    case 2:
                        window.radioButtonAnswerFirst.Content = testOneAnswer.ThreeAnswer;
                        window.radioButtonAnswerSecond.Content = testOneAnswer.SecondAnswer;
                        window.radioButtonAnswerThree.Content = testOneAnswer.TrueAnswer;
                        window.radioButtonAnswerFour.Content = testOneAnswer.FourAnswer;
                        break;
                    case 3:
                        window.radioButtonAnswerFirst.Content = testOneAnswer.ThreeAnswer;
                        window.radioButtonAnswerSecond.Content = testOneAnswer.SecondAnswer;
                        window.radioButtonAnswerThree.Content = testOneAnswer.FourAnswer;
                        window.radioButtonAnswerFour.Content = testOneAnswer.TrueAnswer;
                        break;
                }
                window.Show();
            }
            else if (test is Data.TestManyAnswers testManyAnswer)
            {
                WindowManyAnswers window = new WindowManyAnswers();
                window.checkBoxs = new CheckBox[]{
                                window.checkBoxAnswerFirst,
                                window.checkBoxAnswerSecond,
                                window.checkBoxAnswerThree,
                                window.checkBoxAnswerFour
                            };

                // write name and question
                window.textBlockName.Text = Data.currentName;
                window.textBlockQuestion.Text = $"Вопрос #{Data.questionNumber}: {testManyAnswer.Question}";

                // write answers
                int countAnswer = 4;
                if (testManyAnswer.ThreeAnswer == null)
                {
                    countAnswer = 2;
                    window.checkBoxAnswerThree.Visibility = Visibility.Collapsed;
                    window.checkBoxAnswerFour.Visibility = Visibility.Collapsed;
                }
                else if (testManyAnswer.FourAnswer == null)
                {
                    countAnswer = 3;
                    window.checkBoxAnswerFour.Visibility = Visibility.Collapsed;
                }
                string[] answers = { testManyAnswer.FirstAnswer, testManyAnswer.SecondAnswer, testManyAnswer.ThreeAnswer, testManyAnswer.FourAnswer };
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
                        window.checkBoxs[index].Content = answers[i];
                        listCloseCheckBoxes.Add(index);
                    }
                }
                window.Show();
            }
        }


    }
}
