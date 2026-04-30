using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Calculator_3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ITextBoxEditor textBoxEditor;
        ICalculateText calculateText;
        public MainWindow()
        {
            InitializeComponent();
            textBoxEditor = new TextBoxEditor(textBoxMain);
            calculateText = new CalculateText();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            String textOperation = button.Content.ToString();
            char charOperation = textOperation[0];

            switch (charOperation)
            {
                case '=':
                    string text = textBoxMain.Text;
                    string result = calculateText.Calculate(text);
                    if (ValidationNumber.Validation(result) && result != text)
                        textBoxResult.Text = $"= {result}";
                    else
                        textBoxResult.Text = $"Ошибка в выражении";
                    break;
                case 'C':
                    textBoxEditor.Clear();
                    textBoxResult.Text = "";
                    break;
                case 'd':
                    textBoxEditor.DeleteChar();
                    break;
                default:
                    textBoxEditor.AddChar(textOperation[0]);
                    break;
            }
        }

        interface ITextBoxEditor
        {
            void AddChar(char c);
            void DeleteChar();
            void Clear();
        }
        class TextBoxEditor : ITextBoxEditor
        {

            TextBox textBox;
            public TextBoxEditor(TextBox textBoxMain)
            {
                this.textBox = textBoxMain;
            }

            private void GetParams(out int cursor, out StringBuilder stringBuilder)
            {
                cursor = textBox.CaretIndex;
                string text = textBox.Text;
                stringBuilder = new StringBuilder(text);
            }

            private void UpdateParams(int cursor, StringBuilder stringBuilder)
            {
                var textResult = stringBuilder.ToString();
                textBox.Text = textResult;
                textBox.CaretIndex = cursor;
            }

            public void AddChar(char ch)
            {
                GetParams(out int cursor, out StringBuilder textBuilder);
                textBuilder.Insert(cursor, ch);
                cursor++;
                UpdateParams(cursor, textBuilder);
            }

            public void DeleteChar()
            {
                GetParams(out int cursor, out StringBuilder textBuilder);
                if (cursor == 0)
                    return;
                textBuilder.Remove(cursor - 1, 1);
                cursor--;
                UpdateParams(cursor, textBuilder);
            }

            public void Clear()
            {
                GetParams(out int cursor, out StringBuilder textBuilder);
                textBuilder.Clear();
                UpdateParams(cursor, textBuilder);
            }
        }

        interface ICalculateText
        {
            string Calculate(string text);
        }
        class CalculateText : ICalculateText
        {
            public string Calculate(string text)
            {
                OneNumberPercent(ref text);

                while (MathElementFinder.FindFirstOparetion(text, out string strNumFirst, out string operation, out string strNumSecond, out string allString))
                {
                    Percent(ref strNumFirst, ref operation, ref strNumSecond);
                    DoublesParser.Parse(strNumFirst, strNumSecond, out double numFirst, out double numSecond);
                    var textBuilder = new StringBuilder(text);

                    double result = 0;
                    switch (operation)
                    {
                        case "*":
                            result = numFirst * numSecond;
                            break;
                        case "/":
                            result = numFirst / numSecond;
                            break;
                        case "-":
                            result = numFirst - numSecond;
                            break;
                        case "+":
                            result = numFirst + numSecond;
                            break;
                    }

                    textBuilder.Replace(allString, result.ToString("0.##"));
                    text = textBuilder.ToString();
                }
                //Show(text);
                // 34.14% * -371
                // 1000 - 20% + 50 
                // 300 / 30 * 5 + 35 * 2 / 5
                // 345 + 2342 / 2343 * 23424

                return text;
            }

            private void OneNumberPercent(ref string number)
            {
                if (Regex.IsMatch(number, @"^ *\-?\d+\,?\d*\% *$"))
                {
                    number = number.Substring(0, number.Length - 1);
                    var integer = double.Parse(number);
                    if (integer < 1)
                        integer *= 100;
                    else
                        integer /= 100;
                    number = "" + integer;
                }
            }
            private void Percent(ref string numberFirst, ref string operation, ref string numberSecond)
            {
                if (numberFirst.EndsWith("%"))
                {
                    double first = double.Parse(numberFirst.Substring(0, numberFirst.Length - 1));
                    if (first > 1)
                        numberFirst = "" + (first / 100);
                    else
                        numberFirst = "" + (first * 100);
                }
                if (numberSecond.EndsWith("%"))
                {
                    double first = double.Parse(numberFirst);
                    double second = double.Parse(numberSecond.Substring(0, numberSecond.Length - 1));
                    switch (operation)
                    {
                        case "/":
                        case "*":
                            numberSecond = "" + (second / 100);
                            break;
                        case "-":
                        case "+":
                            numberSecond = "" + (second / 100 * first);
                            break;
                    }
                }
            }

            static class MathElementFinder
            {
                const string P_NUMBER = @"\-?\d+\,?\d*\%?(?!\d)";
                const string P_FIRST_OPERATION = @"[\*/]";
                const string P_SECOND_OPERATION = @"[\-+]";
                const string P_SPACE = @" *";
                public static bool FindFirstOparetion(string text, out string numberFirst, out string operation, out string numberSecond, out string allString)
                {
                    Match match = Regex.Match(text, $"({P_NUMBER}){P_SPACE}({P_FIRST_OPERATION}){P_SPACE}({P_NUMBER})");
                    if (match.Success == false)
                        match = Regex.Match(text, $"({P_NUMBER}){P_SPACE}({P_SECOND_OPERATION}){P_SPACE}({P_NUMBER})");
                    string expression = match.Value;
                    numberFirst = match.Groups[1].Value;
                    operation = match.Groups[2].Value;
                    numberSecond = match.Groups[3].Value;
                    allString = match.Value;
                    return match.Success;
                }
            }
            static class DoublesParser
            {
                static public void Parse(string text1, string text2, out double intFirst, out double intSecond)
                {
                    intFirst = double.Parse(text1);
                    intSecond = double.Parse(text2);
                }
            }
        }
        class ValidationNumber
        {
            const string P_NUMBER = @"^\-?\d+\,?\d*$";
            public static bool Validation(string text)
            {
                if (Regex.IsMatch(text, P_NUMBER))
                    return true;
                return false;
            }
        }
        private static void Show(string text)
        {
            MessageBox.Show(text);
        }

        private void textBoxMain_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            string text = e.Text;
            e.Handled = !Regex.IsMatch(text, @"[-+*/0-9 %,]");
        }

        private void textBoxResult_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = true;
        }

        private void textBoxMain_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                Button button = new Button();
                button.Content = "=";
                Button_Click(button, e);
            }
        }
    }


}


