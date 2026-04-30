using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Calculate
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public enum Operation
    {
        NULL,
        ADDITION = '+',
        SUBSTRUCT = '-',
        DIVISION = '/',
        MULTIPLICATION = '*'
    }
    public partial class MainWindow : Window
    {
        public Operation currentOperation = Operation.NULL;
        public MainWindow()
        {
            InitializeComponent();
        }

        public void AddNumber(char number)
        {

            if (currentOperation == Operation.NULL)
            {
                string content = textFirstNumber.Text;
                content += number;
                textFirstNumber.Text = content;
            }
            else
            {
                string content = textSecondNumber.Text.ToString();
                content += number;
                textSecondNumber.Text = content;
            }
        }

        public void SetOperation(Operation operation)
        {
            currentOperation = operation;
            textOperations.Text = "" + (char)operation;
        }

        public string TryReadInputs(out double furstNumber, out double secondNumber)
        {
            // Replace . on ,
            textFirstNumber.Text = textFirstNumber.Text.Replace(".", ",");
            textSecondNumber.Text = textSecondNumber.Text.Replace(".", ",");

            furstNumber = 0;
            secondNumber = 0;
            try
            {
                furstNumber = double.Parse(textFirstNumber.Text.ToString());
            }
            catch
            {
                return "Не корректный\n ввод поля А";
            }

            try
            {
                secondNumber = double.Parse(textSecondNumber.Text.ToString());
            }
            catch
            {
                return "Не корректный\n ввод поля B";
            }

            if (secondNumber == 0 && currentOperation == Operation.DIVISION)
            {
                return "Деление\n на 0";
            }

            return null;
        }

        public double Calculate(double firstNumber, double secondNumber, Operation operation)
        {
            switch (operation)
            {
                case Operation.ADDITION:
                    return firstNumber + secondNumber;
                case Operation.SUBSTRUCT:
                    return firstNumber - secondNumber;
                case Operation.MULTIPLICATION:
                    return firstNumber * secondNumber;
                case Operation.DIVISION:
                    return firstNumber / secondNumber;
            }
            return 0;
        }

        public void ShowResult(string result)
        {
            textResult.Text = $"={result}";
        }

        public void ShowError(string error)
        {
            textStatus.Text = $"Ошибка\n{error}";
        }

        public void ClearAll()
        {
            textFirstNumber.Text = "";
            textOperations.Text = "";
            textSecondNumber.Text = "";
            textStatus.Text = "";
            textResult.Text = "";
            currentOperation = Operation.NULL;
        }

        public void UpdateMessage()
        {
            
        }

        private void Button_Click_0(object sender, RoutedEventArgs e)
        {
            AddNumber('0');
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AddNumber('1');

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            AddNumber('2');

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            AddNumber('3');

        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            AddNumber('4');

        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            AddNumber('5');

        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            AddNumber('6');

        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            AddNumber('7');

        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            AddNumber('8');

        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            AddNumber('9');

        }

        private void Button_Click_result(object sender, RoutedEventArgs e)
        {
            double firstNumber;
            double secondNumber;
            string error = TryReadInputs(out firstNumber, out secondNumber);
            if (error != null)
            {
                ShowError(error);
            }
            else
            {
                double result = Calculate(firstNumber, secondNumber, currentOperation);
                ShowResult("" + result);
            }
        }

        private void Button_Click_Addition(object sender, RoutedEventArgs e)
        {
            SetOperation(Operation.ADDITION);
        }

        private void Button_Click_Subtract(object sender, RoutedEventArgs e)
        {
            SetOperation(Operation.SUBSTRUCT);
        }

        private void Button_Click_Multiplication(object sender, RoutedEventArgs e)
        {
            SetOperation(Operation.MULTIPLICATION);
        }

        private void Button_Click_Division(object sender, RoutedEventArgs e)
        {
            SetOperation(Operation.DIVISION);
        }

        private void Button_Click_Clear(object sender, RoutedEventArgs e)
        {
            ClearAll();
        }

        private void textOperations_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            string text = textOperations.Text;
            switch (text)
            {
                case "+":
                    currentOperation = Operation.ADDITION;
                    break;
                case "-":
                    currentOperation = Operation.SUBSTRUCT;
                    break;
                case "*":
                    currentOperation = Operation.MULTIPLICATION;
                    break;
                case "/":
                    currentOperation = Operation.DIVISION;
                    break;
                case "":
                    currentOperation = Operation.NULL;
                    break;
                default:
                    textOperations.Text = "";
                    break;
            }
        }

        private void Button_Click_Point(object sender, RoutedEventArgs e)
        {
            AddNumber(',');
        }

        public void ParseTextBoxNumber(TextBox textBox)
        {
        }

        private void textSecondNumber_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            ParseTextBoxNumber(textFirstNumber);
        }

        private void textFirstNumber_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            ParseTextBoxNumber(textSecondNumber);
        }
    }
}
